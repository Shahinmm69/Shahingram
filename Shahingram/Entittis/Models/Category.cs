﻿using Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Category : Modification
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<User>? Users { get; set; }
    }
}
