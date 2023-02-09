namespace SIEG_API.DTO
{
    public class D_GradeBuyDTO
    {
        public int? BuyerId { get; set; }
        public int? SellerId { get; set; }
        public string? State { get; set; } 
        public int? Price { get; set; } 
        public int BuyerGrade { get; set; } 
        public int SellerGrade { get; set; } 
        public int? Count { get; set; } 
    }
}
