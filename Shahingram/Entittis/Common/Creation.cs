using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    public abstract class Creation : BaseEntity
    {
        public DateTime CreationDate { get; set; }
        public int UserCreationId { get; set; }
    }
}
