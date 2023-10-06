using Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Entittis
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