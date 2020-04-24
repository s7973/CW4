using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTO.Requests
{
    public class EnrollStudentRequest
    {

        
        [Required(ErrorMessage = "400")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "400")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "400")]
        public String IndexNumber { get; set; }

        [Required(ErrorMessage = "400")]
        public DateTime BirthDate { get; set; }
        
        [Required(ErrorMessage = "400")]
        public string Studies { get; set; }

        public string Semester { get; set; }

    }
}
