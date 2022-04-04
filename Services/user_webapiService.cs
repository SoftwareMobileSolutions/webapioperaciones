using webapioperaciones.Interfaces;
using webapioperaciones.Models;
using webapioperaciones.Data;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapioperaciones.Services
{
    public class user_webapiService : Iuser_webapi
    {
        private readonly SqlCnConfigMain _configuration;
        public user_webapiService(SqlCnConfigMain configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<user_webapiModel>> getUserAccess(string usuario, string userkey)
        {
            IEnumerable<user_webapiModel> data;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                string query = @"exec webapirest_getUserAccess @usuario, @userkey";

                if (conn.State == ConnectionState.Closed) {
                    conn.Open();
                }
                data = await conn.QueryAsync<user_webapiModel>(query, new { usuario, userkey }, commandType: CommandType.Text);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
            }
            return data;
        }
    }
}
