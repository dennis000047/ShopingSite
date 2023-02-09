namespace SIEG_API.DTO
{
    public class B_BuyerBidsDTO
    {
        public int BuyerBidId { get; set; }
        public int MemberId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImgFront { get; set; }
        public int Price { get; set; }
        public int lowPrice { get; set; }
        public string? Size { get; set; }
        public DateTime? BidTime { get; set; }
        public string? Model { get; set; }
        public int? SellerAddProductID { get; set; }

        public int? FinalPrice { get; set; }
    }
}
