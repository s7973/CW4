using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Requests;
using WebApplication1.DTO.Responses;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/enrollments")]

    public class EnrollmentsController : ControllerBase
    {

        [HttpPost]

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {



            using (var con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True"))
            using (var com = new SqlCommand())
            using (var com2 = new SqlCommand())
            using (var com3 = new SqlCommand())
            {
                com.Connection = con;
                com2.Connection = con;
                com3.Connection = con;
                con.Open();
                //var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "select IdStudy from studies where name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        // tran.Rollback();
                        return BadRequest(400);
                    }

                    con.Close();
                    con.Open();
                    com2.CommandText = "select semester from enrollment e, studies s where e.idstudy = s.idstudy and s.idstudy = '1' and s.name = @name";
                    com2.Parameters.AddWithValue("name", request.Studies);
                    var dd = com2.ExecuteReader();
                    if (dd.Read())
                    {
                        //tran.Rollback();
                        return BadRequest(401);
                    }

                    con.Close();
                    con.Open();
                    com3.CommandText = "Insert into student (firstname, lastname, birthdate, indexnumber) values ((@firstname = firstname), (@lastname = lastname), (@birthdate = birthdate), (@indexnumber = indexnumber)";
                    com3.Parameters.AddWithValue("firstname", request.FirstName);
                    com3.Parameters.AddWithValue("lastname", request.LastName);
                    com3.Parameters.AddWithValue("indexnumber", request.IndexNumber);
                    com3.Parameters.AddWithValue("birthdate", request.BirthDate);



                    com.ExecuteNonQuery();


                    // tran.Commit();

                }
                catch (SqlException e)
                {
                    //  tran.Rollback();
                }

            }

            return Ok(201);
        }

        [Route("api/enrollments/promotions")]

        [HttpPost]

        public IActionResult PromoteStudent(PromoteStudents request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True"))
            using (var com = new SqlCommand())
            using (var proc = new SqlCommand("procedura"))
            {
                com.Connection = con;
                proc.Connection = con;
                con.Open();

                try
                {
                    com.CommandText = "select * from enrollment e, studies s where e.idstudy = s.idstudy and name = @name and semester = @semester";
                    com.Parameters.AddWithValue("name", request.Studies);
                    com.Parameters.AddWithValue("semester", request.Semester);
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        return BadRequest("404 Not Found");
                    }
                    proc.CommandType = System.Data.CommandType.StoredProcedure;
                    proc.Parameters.Add(new SqlParameter("@STUDY", request.Studies));
                    proc.Parameters.Add(new SqlParameter("@SEMESTER", request.Semester));


                }
                catch (SqlException e)
                {
                   
                }

                return Ok();
            }
        }


    }
}
