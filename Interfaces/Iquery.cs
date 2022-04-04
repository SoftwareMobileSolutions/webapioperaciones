using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapioperaciones.Interfaces
{
    public interface Iquery
    {
        Task<IEnumerable<dynamic>> queries(string idoperacion, string parametros);
    }
}
