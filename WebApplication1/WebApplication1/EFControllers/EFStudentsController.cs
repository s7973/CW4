using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WebApplication1.EFDTO.Requests;
using WebApplication1.EFModels;

namespace WebApplication1.EFControllers
{

    [ApiController]

    public class EFStudentsController : ControllerBase
    {

        [Route("api/enrollments/EF")]
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            var db = new s7973Context();
            var res = db.Student.ToList();

            return res;

        }

        [Route("api/enrollments/EF/update/{index}")]
        [HttpPost]

        public IActionResult Update([FromRoute] String index)
        {
            var db = new s7973Context();
            var stud = db.Student.SingleOrDefault(x => x.IndexNumber == index);

            stud.FirstName = "Rafał";
            stud.LastName = "Smoczyński";

            db.SaveChangesAsync();
            return Ok(stud);
        }

        [Route("api/enrollments/EF/delete/{index}")]
        [HttpPost]

        public IActionResult Delete([FromRoute] String index)
        {
            var db = new s7973Context();
            var stud = db.Student.SingleOrDefault(x => x.IndexNumber == index);

            db.Student.Remove(stud);

            db.SaveChangesAsync();

            return Ok("Student został usuniety");
        }

        [Route("api/enrollments/EF/EnrollStudent")]
        [HttpPost]
        public IActionResult EnrollStudent(EFEnrollSudentRequest request)
        {
            var db = new s7973Context();
            var studies = db.Studies.SingleOrDefault(x => x.Name == request.Studies);

            if (studies == null) { return BadRequest(400); }

            var semester = (from Studies in db.Studies join enrollment in db.Enrollment on Studies.IdStudy equals enrollment.IdStudy where Studies.Name == request.Studies && enrollment.Semester == 1 select new { Studies.Name, enrollment.Semester });

            var c = (semester.SingleOrDefault(x => x.Semester == 1));

            if (c == null)
            {
                var stud = db.Studies.SingleOrDefault(x => x.Name == request.Studies);
                var enroll = new Enrollment()
                {
                    IdEnrollment = 40,
                    Semester = 1,
                    StartDate = DateTime.Now,
                    IdStudy = stud.IdStudy

                };
                db.Enrollment.Add(enroll);
                db.SaveChanges();
            }

            var idx = db.Student.SingleOrDefault(x => x.IndexNumber == request.IndexNumber);
            if (idx == null)
            {
                var enroll = db.Enrollment.SingleOrDefault(x => x.StartDate == DateTime.Now.Date);
                var stud = new Student()
                {
                    IndexNumber = request.IndexNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    IdEnrollment = enroll.IdEnrollment
                };
                db.Student.Add(stud);
                db.SaveChanges();
            }
            else { return StatusCode(400, "Student znajduje się już bazie"); }

            var enr = db.Enrollment.SingleOrDefault(x => x.StartDate == DateTime.Now.Date);
            return StatusCode(201, enr);

        }

        [Route("api/enrollments/EF/Promotions")]
        [HttpPost]
        public IActionResult PromoteStudent(EFPromoteStudents request)
        {
            var db = new s7973Context();
            var stud = new EFPromoteStudents()
            {
                Studies = request.Studies,
                Semester = request.Semester
            };

            var studies = (from Studies in db.Studies join enrollment in db.Enrollment on Studies.IdStudy equals enrollment.IdStudy where Studies.Name == request.Studies && enrollment.Semester == request.Semester select new { Studies.Name, enrollment.Semester });
            var c = (studies.SingleOrDefault(x => x.Semester == request.Semester));

            if (c == null)
            {
                return StatusCode(404, "Not found");
            }

            var test = "Informatyka";
            var test2 = 6;
            var proc = db.Database.ExecuteSqlCommand("PromoteStudents @p0, @p1", stud.Studies, stud.Semester);

            db.SaveChanges();
            var enr = db.Enrollment.SingleOrDefault(x => x.StartDate == DateTime.Now.Date);

            return StatusCode(201, enr);
        }

    }
}