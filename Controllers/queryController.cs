using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webapioperaciones.Interfaces;
using System.Linq;
using System;

namespace webapioperaciones.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class queryController : ControllerBase
    {
        Iquery Iquery_;
        Iuser_webapi Iuser_webapi_;
        public queryController(Iquery _Iquery_, Iuser_webapi _Iuser_webapi_)
        {
            Iquery_ = _Iquery_;
            Iuser_webapi_ = _Iuser_webapi_;
        }

        [HttpGet]
        [Route("{usuario}/{accesskey}/{operacionid}/{paramsvalue?}")] //a0x es el alias para llamarle a la funcion consulta
        public async Task<JsonResult> consulta(string usuario = "", string accesskey = "", string operacionid = "", string paramsvalue = "")
        {
            try
            {
                bool access = (await Iuser_webapi_.getUserAccess(usuario, accesskey)).FirstOrDefault().resultado;
                if (access)
                {
                   var p =
                   paramsvalue.Split(",")//.AsParallel()
                   .Where(x => !string.IsNullOrEmpty(x))
                   .Select(x => x.Split(","))
                   .Select
                   (
                       x => Tuple.Create
                       (
                           x[0].Trim().StartsWith("ff") ?
                           "'" + x[0].Trim().Replace("ff", "").Replace("..", ":") + "'" :
                           x[0].Trim()
                       )
                   )
                    .ToList();

                    string parametros = string.Join(",", p.Select(x => x.Item1));

                    var data = await Iquery_.queries(operacionid, parametros);
                    return await Task.Run(() => {
                        return new JsonResult(data);
                    });
                } else {
                    return new JsonResult(StatusCode(401));
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(BadRequest(ex.Message));
            }

        }
    }
}