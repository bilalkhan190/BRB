using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class ResponsibilityOptionsResponse
    {
        public int ResponsibilityOptionsResponseId { get; set; }
        public int PositionId { get; set; }
        public int ResponsibilityOption { get; set; }
    }
}
