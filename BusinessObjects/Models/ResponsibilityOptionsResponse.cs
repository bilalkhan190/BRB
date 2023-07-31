using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public string Caption { get; set; }
        public int ResponsibilityOption { get; set; }
    }
}
