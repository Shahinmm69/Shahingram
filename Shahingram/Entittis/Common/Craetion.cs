using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    public abstract class Craetion : BaseEntity
    {
        public DateTime CrationDate { get; set; }
        public int UserCraetionId { get; set; }
    }
}
