using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class LanguageViewModel : CommonModel
    {
        public int LanguageSkillId { get; set; }

        public int ResumeId { get; set; }

        public bool IsComplete { get; set; }

        public bool IsOptOut { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }

        public List<Language> Languages { get; set; } = new List<Language>();
    }
}
