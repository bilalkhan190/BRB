using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
   
    public interface ITechnicalSkillService
    {
        TechnicalSkill AddTechnicalSkills(TechnicalSkillViewModel technicalSkill);

       
    }
}
