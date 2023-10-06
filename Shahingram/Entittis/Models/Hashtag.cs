using Entities.Common;
using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Hashtag : Modification, IDeletion
    {
        [Required]
        public string Address { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public virtual ICollection<PostHashtag> PostHashtags { get; set; }
        public virtual ICollection<CommentHashtag> CommentHashtags { get; set; }
    }
}