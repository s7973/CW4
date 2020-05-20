using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Student
    {

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String IndexNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public int IdEnrollment { get; set; }

        public String Password { get; set; }

        public String refToken { get; set; }

    }

    public class Student2
    {

        public String Semester { get; set; }

        
    }
    
    public class StudentLogin
    {
        public String IndexNumber { get; set; }

        public String refToken { get; set; }
    }
    

}
