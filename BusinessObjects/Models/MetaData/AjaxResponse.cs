using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models.DTOs
{
    public class AjaxResponse
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
        public bool Error { get; set; }
        public string Redirect { get; set; }
    }
}
