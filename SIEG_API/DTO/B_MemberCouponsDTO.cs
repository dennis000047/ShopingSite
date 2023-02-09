namespace SIEG_API.DTO
{
    public class B_MemberCouponsDTO
    {
        public int? MemberId { get; set; }
        public string? CouponName { get; set; }
        public int CouponId { get; set; }
        public int? count { get; set; }
        public string? SerialNumber { get; set; }
        public int? DiscountPrice { get; set; }
    }
}
