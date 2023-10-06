using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class HashtagDto : BaseDto<HashtagDto, Hashtag>
    {
        public string Address { get; set; }
    }
}
