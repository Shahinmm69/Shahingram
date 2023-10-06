using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class PostDto : BaseDto<PostDto, Post>
    {
        public string Text { get; set; }
        public int UserId { get; set; }

        public string PhotoAddress { get; set; }
        public string VideoAddress { get; set; }
    }

    public class PostSelectDto : BaseDto<PostSelectDto, Post>
    {
        public string Text { get; set; }
        public int UserName { get; set; }
    }
}
