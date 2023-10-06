using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    public abstract class Modification : Craetion
    {
        public DateTime ModificationDate { get; set; }
        public int UserModificationId { get; set; }
    }
}
