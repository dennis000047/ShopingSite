namespace SIEG_API.DTO
{
    public class G_ForumReplyDTO
    {
        public int ForumReplyId { get; set; }
        public int ForumArticleId { get; set; }
        public int MemberId { get; set; }
        public int Floor { get; set; }
        public string? ForumReplyContent { get; set; }
        public string? Img { get; set; }
        public DateTime AddTime { get; set; }
        public bool? ValIdity { get; set; }
        public int LikeCount { get; set; }
        public string? NickName { get; set; }

    }
}
