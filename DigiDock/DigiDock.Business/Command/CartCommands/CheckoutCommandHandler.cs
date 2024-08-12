using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace DigiDock.Business.Command.CartCommands
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, ApiResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CheckoutCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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
            if (coupon.IsRedeemed == true) return ApiResponse.ErrorResponse("Invalid or already used coupon.");

            var cardCheckResponse = CheckCard(request.Request.CardNumber, request.Request.ExpiryDate, request.Request.CVV);
            if (cardCheckResponse != "Card is valid")
            {
                return ApiResponse.ErrorResponse(cardCheckResponse);
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
                newOrder.OrderDetails.Add(orderDetail);
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
    }
}
