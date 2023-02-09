namespace SIEG_API.DTO
{
    public class E_NewsHotDTO
    {
        public int newslistId { get; set; }
        public string? newslistImg { get; set; }
        public string? newslistTitle { get; set; }
        public string? newslistContent { get; set; }
        public int? newslistSort { get; set; }
        public DateTime? newslistTime { get; set; }
        public int? newslistviewcount { get; set; }
    }
}
