using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entittis
{
    public class Photo : Craetion, IDeletion
    {
        [Required]
        public string Address { get; set; }

        public string Describtion { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<PostPhoto> PostPhotos { get; set; }
        public virtual ICollection<DirectPhoto> DirectPhotos { get; set; }
    }
}
