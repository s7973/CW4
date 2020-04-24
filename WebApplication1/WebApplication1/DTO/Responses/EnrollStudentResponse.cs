using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTO.Responses
{
    public class EnrollStudentResponse
    {
        public String LastName { get; set; }
        public int Semester { get; set; }

        public string Studies { get; set; }

        public DateTime StartDate { get; set; }

    }
}
