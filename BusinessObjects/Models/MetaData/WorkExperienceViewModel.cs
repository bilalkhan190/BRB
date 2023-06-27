using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class WorkExperienceViewModel
    {
        public List<WorkCompany> Companies { get; set; } = new List<WorkCompany>();
        public  WorkPosition Position { get; set; }


    }
}
