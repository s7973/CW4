using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.EFDTO.Requests
{
    public class EFEnrollSudentRequest
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

        public string Name { get; set; }
        
        public int IdStudy { get; set; }
    }
}
