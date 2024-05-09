using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webapioperaciones.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using webapioperaciones.Models;
using System.Data.SqlClient;

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



        [HttpGet, HttpPost]
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


        [HttpGet, HttpPost]
        [Route("consultav4")] //FROM BODY
        public async Task<JsonResult> consultav4(string usuario = "", string accesskey = "", string operacionid = "", string paramsvalue = "")
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
                }
                else
                {
                    return new JsonResult(StatusCode(401));
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(BadRequest(ex.Message));
            }

        }




        [HttpGet]
        [Route("{usuario}/{accesskey}/{operacionid}/v2/{paramsvalue?}")] //a0x es el alias para llamarle a la funcion consulta
        public async Task<JsonResult> consultav2(string usuario = "", string accesskey = "", string operacionid = "", string paramsvalue = "")
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

                    foreach (var v in (data as IEnumerable<object>))
                    {
                        if (v is KeyValuePair<string, string>)
                        {
                            // Do stuff
                        }
                        else if (v is List<string>)
                        {
                            //Do stuff
                        }

                        else throw new InvalidOperationException();
                    }

                    return await Task.Run(() => {
                        return new JsonResult(data);
                    });
                }
                else
                {
                    return new JsonResult(StatusCode(401));
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(BadRequest(ex.Message));
            }

        }

		[HttpGet]
		[Route("{usuario}/{accesskey}/{operacionid}/v3")] //a0x es el alias para llamarle a la funcion consulta
		public async Task<JsonResult> consultav3(string usuario = "", string accesskey = "", string operacionid = "", string paramsvalue = "")
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
				}
				else
				{
					return new JsonResult(StatusCode(401));
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(BadRequest(ex.Message));
			}

		}

		//Resumen del día
		[HttpGet]
       // [Route("{mobileid}")]
        public async Task<JsonResult> ObtenerResumenDiaMobile(int mobileid, string GalonesXHora1=null)
        {
            //var usuario = _Session.Get<IEnumerable<LoginModel>>(HttpContext.Session, "usuario").First();
            // SqlCnUrl SqlCnUrl = new SqlCnUrl(ctx);
            string strODBC = "DSN=Bluefenyx;UID=mantenimiento;PWD=mttofleet;"; //SqlCnUrl.getODBC(0);

            Summary.Summ ResumenReco = new Summary.Summ();
            List<DatosResumenDiaModel> lDatosResDia = new List<DatosResumenDiaModel>();
            string strFechainicio, strFechafin, strError = "";
            double MaxSpeed, AvgSpeed, SumDistTot, TimeProd, TimeRecoProd, TimeDetProd, TimeDetVehON, MaxTimeStopOn, Kmconsumidos;
            int NumParadas;

            DateTime dtFH = DateTime.Now.AddHours(-18);
            DateTime dtFechaHoy = new DateTime(dtFH.Year, dtFH.Month, dtFH.Day, 6, 0, 0);

            strFechainicio = dtFechaHoy.ToString("dd-MM-yyyy HH:mm:ss");
            //strFechafin = DateTime.Now.AddHours(6).ToString("dd-MM-yyyy HH:mm:ss");
            strFechafin = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string strMobileid = Convert.ToString(mobileid);
            MaxSpeed = 0;
            AvgSpeed = 0;
            SumDistTot = 0;
            TimeDetProd = 0;
            TimeDetVehON = 0;
            TimeProd = 0;
            TimeRecoProd = 0;
            NumParadas = 0;
            MaxTimeStopOn = 0;
            Kmconsumidos = 0;

            List<DatosResumenDiaModel> DatosResumenDia = new List<DatosResumenDiaModel>();
            if (ResumenReco.ObtenerResumen(strODBC, strFechainicio, strFechafin, strMobileid, ref MaxSpeed, ref AvgSpeed, ref SumDistTot, ref NumParadas, ref TimeProd, ref TimeRecoProd, ref TimeDetProd, ref TimeDetVehON, ref MaxTimeStopOn, ref Kmconsumidos, ref strError) == 1)
            {
                DatosResumenDia.Add(new DatosResumenDiaModel
                {
                    MaxSpeed_Title = "Máxima Velocidad (km/h):",
                    MaxSpeed = Math.Round(MaxSpeed, 2),

                    SumDistTot_Title = "Distancia Total (km):",
                    SumDistTot = Math.Round(SumDistTot, 2),

                    TimeProd_Title = "Tiempo Producción (min):",
                    TimeProd = Math.Round(TimeProd, 2),

                    TimeRecoProd_Title = "Tiempo Recorrido en Producción (min):",
                    TimeRecoProd = Math.Round(TimeRecoProd, 2),

                    TimeDetProd_Title = "Tiempo Detenido en Producción (min):",
                    TimeDetProd = Math.Round(TimeDetProd, 2),

                    TimeDetVehON_Title = "Ralentí (min):",
                    TimeDetVehON = Math.Round(TimeDetVehON, 2),

                    MaxTimeStopOn_Title = "Máximo Ralentí (min):",
                    MaxTimeStopOn = Math.Round(MaxTimeStopOn, 2),

                    Kmconsumidos_Title = "Combustible Consumido por Kilometraje Recorrido (gal):",
                    Kmconsumidos = Math.Round(Kmconsumidos, 2),

                    CombConsumidos_Title = "Combustible Consumido por Tiempo Encendido (gal):",
                    CombConsumidos = Math.Round(((TimeRecoProd + TimeDetVehON) / 60) * Convert.ToDouble(GalonesXHora1))
                });

            }
            return await Task.Run(() =>
            {
                return new JsonResult(DatosResumenDia);
            });

        }

    }
}