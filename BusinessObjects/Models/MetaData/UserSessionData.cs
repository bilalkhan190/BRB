using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class UserSessionData
    {
        public UserSessionData()
        {

        }
        public int UserId { get; set; }
        public string? UserType { get; set; }
        public int ResumeId { get; set; }

        public string? UserName  { get; set; }

        public string Ids { get; set; }
        public string VoucherCode { get; set; }

        public int LastSectionVisitedId { get; set; }
        public int LastSectionCompletedId  { get; set; }

    }

    public class TableIdentities
    {
        public int contactInfoId { get; set; }
        public int objectiveId { get; set; }
        public int educationId { get; set; }
        public int overseasExpId { get; set; }
        public int workExpId { get; set; }
        public int militaryExpId { get; set; }
        public int volunteerId { get; set; }
        public int technicalSkillId { get; set; }
        public int professionalId { get; set; }
        public int languageSkillId { get; set; }
        public int orgExperienceId { get; set; }
    }
}
