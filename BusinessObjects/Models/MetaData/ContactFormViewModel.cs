using BusinessObjects.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class ContactInfoViewModel : CommonModel
    {
        public int ContactInfoId { get; set; }

        public int ResumeId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? StateAbbr { get; set; }

        public string? ZipCode { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public bool IsComplete { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
    }
}
