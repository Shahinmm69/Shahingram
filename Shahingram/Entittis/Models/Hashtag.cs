using Entities.Common;
using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Hashtag : Creation, IDeletion
    {
        [Required]
        public string Title { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public virtual ICollection<PostHashtag>? PostHashtags { get; set; }
        public virtual ICollection<CommentHashtag>? CommentHashtags { get; set; }
    }
}