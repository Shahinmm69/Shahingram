﻿using Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entittis
{
    public class Country : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}