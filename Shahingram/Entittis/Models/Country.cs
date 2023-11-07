using Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models
{
    public class Country : Modification
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }
}
