namespace SIEG_API.DTO
{
    public class J_OrderInfo
    {
        public string? info { get; set; }
        public int bID { get; set; }
        public int sID { get; set; }
        public int pID { get; set; }
        public int? buyerPrice { get; set; }
        public int? sellerPrice { get; set; }
        public string? pImg { get; set; }
        public string? pay { get; set; }
        public string? receiver { get; set; }
        public string? receivingPhone { get; set; }
        public string? shippingAddress { get; set; }
        public int finalPrice { get; set; }
        public int? quoteID { get; set; }
        public int? bidID { get; set; }
    }
}
