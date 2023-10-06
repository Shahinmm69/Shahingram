using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class DirectDto : BaseDto<DirectDto, Direct>
    {
        public string Text { get; set; }
        public int UserSenderId { get; set; }
        public int UserReceiverId { get; set; }
        public int PostId { get; set; }

        public string PhotoAddress { get; set; }
        public string VideoAddress { get; set; }
    }

    public class DirectSelectDto : BaseDto<DirectSelectDto, Direct>
    {
        public string Text { get; set; }
        public int UserSenderName { get; set; }
        public int UserReceiverName { get; set; }
    }
}
