namespace SIEG_API.DTO
{
    public class B_ReturnDTO
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int? BuyerPrice { get; set; }
        public int? SellerId { get; set; }
        public int? SellerPrice { get; set; }
        public DateTime? DoneTime { get; set; }
        public string? State { get; set; }
        public string? Image { get; set; }
        public string? Model { get; set; }
        public string? ProductName { get; set; }
        public string? SizeId { get; set; }
    }
}
