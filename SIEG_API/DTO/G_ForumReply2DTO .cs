namespace SIEG_API.DTO
{
    public class G_ForumReply2DTO
    {
        public int ForumReply2Id { get; set; }
        public int ArticleId { get; set; }
        public int ForumReplyId { get; set; }
        public int MemberId { get; set; }
        public int ForumReplyFloor { get; set; }
        public int Floor { get; set; }
        public string? ForumReply2Content { get; set; }
        public string? Img { get; set; }
        public DateTime AddTime { get; set; }
        public bool? ValIdity { get; set; }
        public int LikeCount { get; set; }
        public string? NickName { get; set; }

    }
}
