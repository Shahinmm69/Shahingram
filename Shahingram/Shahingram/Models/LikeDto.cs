using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class LikeDto : BaseDto<LikeDto, Like>
    {
        public int UserId { get; set; }
        public int FollowId { get; set; }
    }
}
