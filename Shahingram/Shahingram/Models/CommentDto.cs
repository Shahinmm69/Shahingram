using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class CommentDto : BaseDto<CommentDto, Comment>
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int ReplyId { get; set; }
    }

    public class CommentSelectDto : BaseDto<CommentSelectDto, Comment>
    {
        public string Text { get; set; }
        public int UserName { get; set; }
        public string? ReplyText { get; set; }
        public string? ReplyUserName { get; set; }
    }
}
