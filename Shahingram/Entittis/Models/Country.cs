using Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Country : Modification
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<User>? Users { get; set; }
    }
}
