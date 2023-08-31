using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.MetaData
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }


        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string UserPassword { get; set; } = null!;
        [NotMapped]
        [Required]
        [CompareAttribute("UserPassword", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; } = null!;
        //public string Email { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [StringLength(50,ErrorMessage = "address should be consist 50 characters")]
        public string? Address1 { get; set; }
      
       
        public string? Address2 { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? StateAbbr { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "zip code should be consist  5 characters")]
        public string? ZipCode { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "phone number should be consist on  10 characters")]
        public string? Phone { get; set; }
    }
}
