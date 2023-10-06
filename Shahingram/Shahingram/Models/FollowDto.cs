using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class FollowDto : BaseDto<FollowDto, Follow>
    {
        public int UserId { get; set; }
        public int FollowId { get; set; }
    }
}
