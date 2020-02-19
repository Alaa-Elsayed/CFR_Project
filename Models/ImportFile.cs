using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRF_Final_Project.Models
{
    public class ImportFile
    {
        [Required]
        [CustomValid(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
        public HttpPostedFileBase File { get; set; }
    }
}