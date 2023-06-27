using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class ResponsibilityViewModel
    {
        public List<ResponsibilityOption> ResponsibilityOptions { get; set; }
        public List<ResponsibilityQuestion> ResponsibilityQuestions { get; set; }
    }
}
