namespace DigiDock.Schema.Responses
{
    public class OrderResponse
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
    }
}