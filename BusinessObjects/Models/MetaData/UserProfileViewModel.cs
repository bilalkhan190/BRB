using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTOs
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }

       
        public string UserName { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? StateAbbr { get; set; }

        public string? ZipCode { get; set; }

        public string? Phone { get; set; }
    }
}
