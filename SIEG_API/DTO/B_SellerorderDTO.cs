namespace SIEG_API.DTO
{
	public class B_SellerorderDTO
	{
        public string? ImgFront { get; set; }
        public string? ProductName { get; set; }
        public int OrderId { get; set; }
        public int? Price { get; set; }
        public string? Size { get; set; }
        public string? State { get; set; }
        public DateTime? CompleteTime { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Receiver { get; set; }
        public string? Model { get; set; }
    }
}
