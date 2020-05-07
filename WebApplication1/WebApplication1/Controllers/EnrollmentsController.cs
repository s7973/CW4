using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    

    public class EnrollmentsController : ControllerBase
    {
        [Route("api/enrollments")]

        [HttpPost]

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True; MultipleActiveResultSets=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;

                    try
                    {
                    com.CommandText = "select IdStudy from studies where name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        tran.Rollback();
                        return BadRequest(400);
                    }
                    con.Close();
                    con.Open();
                    com.CommandText = "select semester from enrollment e, studies s where e.idstudy = s.idstudy and e.semester = '1' and s.name = @names";
                    com.Parameters.AddWithValue("names", request.Studies);
                    var dd = com.ExecuteReader();

                    if (!dd.HasRows)
                    {
                        con.Close();
                        con.Open();
                        com.CommandText = "insert into enrollment(IdEnrollment, Semester, IdStudy, StartDate) values(40, 1, (select idstudy from studies where name = @name), GETDATE())";
                        com.ExecuteReader();
                    };

                    con.Close();
                    con.Open();
                    com.CommandText = "Select indexnumber from student where indexnumber = @indexnumber2";
                    com.Parameters.AddWithValue("indexnumber2", request.IndexNumber);
                    var ind = com.ExecuteReader();
                    if (!ind.HasRows)
                    {
                        con.Close();
                        con.Open();
                        com.CommandText = "Insert into student (firstname, lastname, birthdate, indexnumber, idenrollment) values (@firstname, @lastname, @birthdate,  @indexnumber3, (select idenrollment from enrollment where startdate = (select max(startdate) from enrollment)))";
                        com.Parameters.AddWithValue("firstname", request.FirstName);
                        com.Parameters.AddWithValue("lastname", request.LastName);
                        com.Parameters.AddWithValue("indexnumber3", request.IndexNumber);
                        com.Parameters.AddWithValue("birthdate", request.BirthDate);
                        com.ExecuteReader();

                        var output = new Enrollment();
                        con.Close();
                        con.Open();
                        com.CommandText = "select * from enrollment where StartDate = (select max(StartDate) from enrollment)";

                        dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            output = new Enrollment
                            {
                                IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                                Semester = int.Parse(dr["Semester"].ToString()),
                                IdStudy = int.Parse(dr["IdStudy"].ToString()),
                                StartDate = DateTime.Parse(dr["StartDate"].ToString())
                            }; ;
                        }
                        return StatusCode(201, output);
                    }
                    else if (ind.HasRows)
                    {
                        return BadRequest("Numer indeksu jest już w bazie");
                    }
                       tran.Commit();
                     }
                     catch (Exception e)
                     {
                          tran.Rollback();
                          return BadRequest(e.Message);
                     }
                }
            return Ok();
            } 

        
        [Route("api/enrollments/promotions")]

        [HttpPost]

        public IActionResult PromoteStudent(PromoteStudents request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True; MultipleActiveResultSets=True" ))
            using (var com = new SqlCommand())
            using (var proc = new SqlCommand("PromoteStudents"))
            {
                com.Connection = con;
                com.CommandText = "select * from enrollment e, studies s where e.idstudy = s.idstudy and name = @name and semester = @semester";
                proc.Connection = con;
                con.Open();

                    com.Parameters.AddWithValue("name", request.Studies);
                    com.Parameters.AddWithValue("semester", request.Semester);
                    var dr = com.ExecuteReader();

                    while (!dr.Read())
                    {
                        return BadRequest("404 Not Found");
                    }
                
                proc.Connection = con;
            
                proc.CommandType = System.Data.CommandType.StoredProcedure;
                    proc.Parameters.Add(new SqlParameter("@STUDIES", request.Studies));
                    proc.Parameters.Add(new SqlParameter("@SEMESTER", request.Semester));

                proc.ExecuteReader();

                var output = new Enrollment();
                con.Close();
                con.Open();
                com.CommandText = "select * from enrollment where StartDate = (select max(StartDate) from enrollment)";

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    output = new Enrollment
                    {
                        IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                        Semester = int.Parse(dr["Semester"].ToString()),
                        IdStudy = int.Parse(dr["IdStudy"].ToString()),
                        StartDate = DateTime.Parse(dr["StartDate"].ToString())
                    }; ;


                }
                return StatusCode(201, output);


            }
        }


    }
}
