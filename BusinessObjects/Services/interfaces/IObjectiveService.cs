using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IObjectiveService
    {
        int AddObjectiveSummary(ObjectiveSummeryViewModel objectiveSummeryViewModel);
    }
}
