using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IMilitaryService
    {
        MilitaryExperience AddMilitaryExperience(MilitaryViewModel militaryViewModel, Wh4lprodContext dbContext);
        MilitaryPosition AddMilitaryPosition(MilitaryPosition militaryPosition, Wh4lprodContext dbContext);

        MilitaryViewModel GetMilitaryExperienceByResumeId(int resumeId);

        MilitaryPosition GetMilitaryPositionByoId(int id);
    }
}