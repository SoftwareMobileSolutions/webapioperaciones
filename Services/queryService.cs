using webapioperaciones.Interfaces;
using webapioperaciones.Data;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace webapioperaciones.Services
{
    public class queryService: Iquery
    {
        private readonly SqlCnConfigMain _configuration;
        public queryService(SqlCnConfigMain configuration) {
            _configuration = configuration;
        }

        public async Task<IEnumerable<dynamic>> queries(string idoperacion, string parametros)
        {
            IEnumerable<dynamic> data;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                string query = @"exec webapirest_execqueries @idoperacion, @parametros";

                if (conn.State == ConnectionState.Closed) {
                    conn.Open();
                }
                data = await conn.QueryAsync<dynamic>(query, new { idoperacion, parametros }, commandType: CommandType.Text);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
            }
            return data;
        }
    }
}
