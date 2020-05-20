using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SaveToken
    {
        String refToken { get; set; }

        private String SqlConn = "Data Source=db-mssql; Initial Catalog=s7973; Integrated Security=True";

        public void Save(String index, String refToken)
        {
            using (var client = new SqlConnection(SqlConn))
            {

                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"update student set refToken = '{refToken}' where indexnumber = '{index}'";

                    client.Open();
                    var dr = command.ExecuteReader();


                }
            }
        }
    }
}
