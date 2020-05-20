using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class CheckToken
    {

        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";

        public StudentLogin Check(String refToken)
        {
            var output = new StudentLogin();
            
            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"select indexnumber, reftoken from student where reftoken = '{refToken}'";

                    client.Open();
                    var dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        output = (new StudentLogin
                        {
                            IndexNumber = dr["IndexNumber"].ToString(),
                            refToken = dr["refToken"].ToString()
                        }); ;

                    }

                }
            }
            return output;
        }

    }
}
