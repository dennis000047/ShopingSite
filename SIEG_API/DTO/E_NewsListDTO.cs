namespace SIEG_API.DTO
{
    public class E_NewsListDTO
    {
        public int newslistId { get; set; }
        public string? newslistImg { get; set; }
        public string? newslistTitle { get; set; }
        public string? newslistContent { get; set; }
        public string? newslistSort { get; set; }
        public DateTime? newslistTime { get; set; }
        public int newslistviewcount { get; set; }
        public bool? newsValIdity { get; set; }
    }
}
