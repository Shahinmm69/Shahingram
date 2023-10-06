using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entittis
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
