using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class LoginDbService
    {
        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";

        public Student GetLoginAndPass(String index, String password)
        {

            var output = new Student();

            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select indexnumber, password from student where indexnumber = '{index}' and password = '{password}'";

                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output = (new Student
                        {
                            IndexNumber = dr["IndexNumber"].ToString(),
                            Password = dr["Password"].ToString()
                        }); ;

                    }
                }
            }
            return output;
            
        }

        public Student GetLoginAndPass(String index)
        {

            var output = new Student();

            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select indexnumber, password from student where indexnumber = '{index}'";

                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output = (new Student
                        {
                            IndexNumber = dr["IndexNumber"].ToString()
                        }); ;

                    }
                }
            }
            return output;
            
        }

        public Pass GetPass(String index)
        {
            var output = new Pass();

            using (var client = new SqlConnection(SqlConn))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select password, salt from student where indexnumber = '{index}'";

                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output = (new Pass
                        {
                            Password = dr["Password"].ToString(),
                            Salt = dr["Salt"].ToString()
                        });
                    }
                }
            }
            return output;
        }
    }
}
