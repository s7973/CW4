using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";

        public Student GetStudent(String index)
        {

            var output = new Student();
            
            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select indexnumber from student where indexnumber = '{index}'";

                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output = (new Student
                        {
                            IndexNumber = dr["IndexNumber"].ToString()
                            
                    });;

                    }
                }
            }
            return output;
            ;
        }

    }
}
