using Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Entittis
{
    public class Video : Craetion, IDeletion
    {
        [Required]
        public string Address { get; set; }

        public string Describtion { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public virtual ICollection<PostVideo> PostVideos { get; set; }
        public virtual ICollection<DirectVideo> DirectVideos { get; set; }
    }
}