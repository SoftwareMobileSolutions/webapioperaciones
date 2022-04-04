using System.Collections.Generic;
using System.Threading.Tasks;
using webapioperaciones.Models;

namespace webapioperaciones.Interfaces
{

    public interface Iuser_webapi {
        Task<IEnumerable<user_webapiModel>> getUserAccess(string usuario, string userkey);
    }
}
