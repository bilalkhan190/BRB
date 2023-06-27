using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTOs
{
    public class ResumeViewModels
    {
        public int ResumeId { get; set; }

        public int UserId { get; set; }

        public int LastSectionVisitedId { get; set; }

        public int LastSectionCompletedId { get; set; }

        public string? VoucherCode { get; set; }

        public bool IsActive { get; set; }

        public string? ChosenFont { get; set; }

        public DateTime? GeneratedDate { get; set; }

        public string? GeneratedFileName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModDate { get; set; }

        public int OfferedProductId { get; set; }

        public string? Data { get; set; }
    }
}
