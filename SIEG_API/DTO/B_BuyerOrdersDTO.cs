namespace SIEG_API.DTO
{
    public class B_BuyerOrdersDTO
    {
        public string? ProductName { get; set; }
        public string? Image { get; set; }
        public string? SizeId { get; set; }
        public int? Price { get; set; }
        public DateTime? CompleteTime { get; set; }
        public DateTime? DoneTime { get; set; }
        public string? ShippingAddress { get; set; }
        public string? State { get; set; }
        public string? Receiver { get; set; }
        public int OrderId { get; set; }
        public string? Model { get; set; }
    }
}
