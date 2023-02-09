namespace SIEG_API.DTO
{
    public class D_NewsInfoDTO
    {
        public int NewsID { get; set; }
        public string? Img { get; set; }
        public string? Title { get; set; }
        public string? NewsContent { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? AddTime { get; set; }
        public int? ViewsCount { get; set; }
    }
}
