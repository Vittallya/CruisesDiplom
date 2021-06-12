using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ConnectorService
    {
        public bool Result { get; private set; }
        public bool IsTryed { get; private set; }

        public int ErrorCode { get; private set; }

        public bool IsConnected { get; private set; }

        private readonly string _str = "Data Source=SQL5097.site4now.net;Initial Catalog=db_a7544b_testdb;User ID=db_a7544b_testdb_admin;password=MyT45en#etBr34";

        public string ErrorMessage { get; private set; }


        public async Task<bool> GetValue(int id)
        {
            using (SqlConnection sqlConnection =
                 new SqlConnection(_str))
            {
                try
                {

                    await sqlConnection.OpenAsync();
                    var com = sqlConnection.CreateCommand();

                    com.CommandText = "SELECT [IsOffDemo] FROM [Table1] WHERE Id=@id";
                    com.Parameters.Add(new SqlParameter("id", id));

                    Result = Convert.ToBoolean(await com.ExecuteScalarAsync());

                    IsTryed = true;
                    IsConnected = true;

                    return Result;
                }
                catch(SqlException ex) when (ex.Number == 53)
                {
                    //53
                    ErrorMessage = "Нет подключения к сети";
                    ErrorCode = ex.Number;
                    IsConnected = false;
                    Result = false;
                    return false;
                }
            }
        }
    }
}
