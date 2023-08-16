using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class EducationViewModel : CommonModel
    {
        public List<College> College { get; set; } = new List<College>();
      

    

        public bool IsComplete { get; set; }
    }
}
