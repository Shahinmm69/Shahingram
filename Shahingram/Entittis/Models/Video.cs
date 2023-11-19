using Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Video : Creation, IDeletion
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string Describtion { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public virtual ICollection<PostVideo>? PostVideos { get; set; }
        public virtual ICollection<DirectVideo>? DirectVideos { get; set; }
    }
}