using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class StudentDbServ : IStudentDbServ
    {
        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";
        public IEnumerable<Student> GetStudents()
        {
            var output = new List<Student>();
            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "SELECT * FROM Student";

                    client.Open();
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        output.Add(new Student
                        {
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IndexNumber = dr["IndexNumber"].ToString(),
                            BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                            IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                        }); ;


                    }
                }
            }
            return output;
        }

        public IEnumerable<Student2> GetStudentById(String id)
        {
            var output = new List<Student2>();
            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select semester from enrollment e, student s where e.IdEnrollment = s.IdEnrollment and IndexNumber = '{id}'";

                    client.Open();
                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        output.Add(new Student2
                        {
                            Semester = dr["Semester"].ToString()
                        }); ;


                    }
                }
            }
            return output;
        }

        
    }
}
