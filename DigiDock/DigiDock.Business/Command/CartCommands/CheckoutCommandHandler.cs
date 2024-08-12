using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Business.Services;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace DigiDock.Business.Command.CartCommands
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EmailQueueService emailQueueService;
        private readonly IConfiguration configuration;

        public CheckoutCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, EmailQueueService emailQueueService, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.emailQueueService = emailQueueService ?? throw new ArgumentNullException(nameof(emailQueueService));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ApiResponse> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (userIdClaim is null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }
            var currentUserId = int.Parse(userIdClaim);

            var user = await unitOfWork.UserRepository.GetByIdAsync(currentUserId);
            if (user == null)
            {
                return ApiResponse.ErrorResponse("User not found.");
            }

            var cart = await unitOfWork.OrderDetailRepository.Where(od => od.UserId == currentUserId && od.OrderId == null, "Product");
            if (cart.Count() == 0)
            {
                return ApiResponse.ErrorResponse("Cart is empty.");
            }

            var coupon = await unitOfWork.CouponRepository.FirstOrDefault(c => c.Code == request.Request.CouponCode);
            if (coupon == null && request.Request.CouponCode != null) return ApiResponse.ErrorResponse("Coupon not found.");
            if (coupon is not null && (coupon.IsRedeemed == true || coupon.ExpiryDate < DateTime.UtcNow)) return ApiResponse.ErrorResponse("Invalid or already used coupon.");

            var cardCheckResponse = CheckCard(request.Request.CardNumber, request.Request.ExpiryDate, request.Request.CVV);
            if (cardCheckResponse != "Card is valid.")
            {
                return ApiResponse.ErrorResponse(cardCheckResponse);
            }

            foreach (var orderDetail in cart)
            {
                if (orderDetail.Product.Stock < orderDetail.Quantity)
                {
                    return ApiResponse.ErrorResponse($"Insufficient stock for product: {orderDetail.Product.Name}");
                }
            }

            decimal totalPrice = cart.Sum(od => od.Quantity * od.Product.Price);

            decimal walletBalance = user.WalletBalance;
            decimal netTotalPrice = totalPrice;

            if (walletBalance > 0)
            {
                if (walletBalance >= totalPrice)
                {
                    netTotalPrice = 0;
                    user.WalletBalance -= totalPrice;
                }
                else
                {
                    netTotalPrice -= walletBalance;
                    user.WalletBalance = 0;
                }
            }

            var newOrder = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CartTotal = totalPrice,
                CouponTotal = coupon != null ? totalPrice * coupon.Discount / 100 : 0,
                CouponCode = coupon?.Code,
                PointTotal = 0,
                OrderDetails = new List<OrderDetail>(),
                UserId = currentUserId
            };

            await unitOfWork.OrderRepository.InsertAsync(newOrder);
            await unitOfWork.CompleteAsync();

            foreach (var orderDetail in cart)
            {
                orderDetail.OrderId = newOrder.Id;
                orderDetail.UnitPrice = orderDetail.Product.Price;
                newOrder.OrderDetails.Add(orderDetail);

                orderDetail.Product.Stock -= (int) orderDetail.Quantity;
                unitOfWork.ProductRepository.Update(orderDetail.Product);
            }

            newOrder.CartTotal = totalPrice - newOrder.CouponTotal;

            decimal earnedPoints = 0;
            foreach (var orderDetail in newOrder.OrderDetails)
            {
                var product = orderDetail.Product;
                decimal productTotal = orderDetail.Quantity * product.Price;
                decimal productPoints = productTotal * product.RewardPointsPercentage / 100;
                earnedPoints += Math.Min(productPoints, product.MaxRewardPoints);
            }

            user.WalletBalance += earnedPoints;
            unitOfWork.UserRepository.Update(user);

            if (coupon != null)
            {
                coupon.IsRedeemed = true;
                unitOfWork.CouponRepository.Update(coupon);
            }

            await unitOfWork.CompleteWithTransactionAsync();

            string emailBody = GenerateOrderEmailBody(user, newOrder);

            emailQueueService.EnqueueEmailTo(
                user.Email,
                "Your Order Confirmation",
                emailBody);

            return ApiResponse.SuccessResponse($"Order created successfully. Total: {newOrder.CartTotal}, Earned Points: {earnedPoints}");
        }

        private string GenerateOrderNumber()
        {
            var random = new Random();
            return random.Next(100000000, 999999999).ToString();
        }

        private string CheckCard(string cardNumber, string expiryDate, string cvv)
        {
            if (!IsValidExpiryDate(expiryDate))
            {
                return "Invalid expiry date.";
            }
            else if (!IsValidCreditCard(cardNumber))
            {
                return "Invalid credit card number.";
            }
            else if (!IsValidCVV(cvv))
            {
                return "Invalid CVV.";
            }

            return "Card is valid.";
        }

        private bool IsValidCreditCard(string cardNumber)
        {
            if (!Regex.IsMatch(cardNumber, @"^\d{13,19}$"))
            {
                return false;
            }
            // Luhn algoritm
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(cardNumber[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        private bool IsValidExpiryDate(string expiryDate)
        {
            if (!Regex.IsMatch(expiryDate, @"^(0[1-9]|1[0-2])\/?([0-9]{2})$"))
            {
                return false;
            }

            var parts = expiryDate.Split('/');
            int month = int.Parse(parts[0]);
            int year = int.Parse(parts[1]) + 2000; 

            var expiry = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            return expiry >= DateTime.Now;
        }

        private bool IsValidCVV(string cvv)
        {
            return Regex.IsMatch(cvv, @"^\d{3,4}$");
        }
        private string GenerateOrderEmailBody(User user, Order order)
        {
            string logoUrl = configuration["LogoUrl"];

            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine($"<div style='text-align: center; margin-bottom: 20px;'>");
            sb.AppendLine($"<img src='{logoUrl}' alt='DigiDock Logo' style='max-width: 100%; height: auto; width: 150px;' />");
            sb.AppendLine("</div>");
            sb.AppendLine("<h1>Order Confirmation</h1>");
            sb.AppendLine($"<p>Dear {user.FirstName} {user.LastName},</p>");
            sb.AppendLine("<p>Thank you for your order. Here are your order details:</p>");
            sb.AppendLine("<ul>");
            sb.AppendLine($"<li><strong>Order Number:</strong> {order.OrderNumber}</li>");
            sb.AppendLine($"<li><strong>Total Price:</strong> {order.CartTotal:C}</li>");
            sb.AppendLine($"<li><strong>Coupon Discount:</strong> {order.CouponTotal:C}</li>");
            sb.AppendLine($"<li><strong>Net Total Price:</strong> {order.CartTotal - order.CouponTotal:C}</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("<h2>Order Items</h2>");
            sb.AppendLine("<ul>");
            foreach (var orderDetail in order.OrderDetails)
            {
                sb.AppendLine("<li>");
                sb.AppendLine($"<strong>Product:</strong> {orderDetail.Product.Name}<br>");
                sb.AppendLine($"<strong>Quantity:</strong> {orderDetail.Quantity}<br>");
                sb.AppendLine($"<strong>Unit Price:</strong> {orderDetail.UnitPrice:C}<br>");
                sb.AppendLine($"<strong>Total:</strong> {orderDetail.Quantity * orderDetail.UnitPrice:C}");
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");
            sb.AppendLine("<p>We hope you enjoy your purchase. If you have any questions, feel free to contact us.</p>");
            sb.AppendLine("<p>Best regards,</p>");
            sb.AppendLine("<p>The DigiDock Team</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}
