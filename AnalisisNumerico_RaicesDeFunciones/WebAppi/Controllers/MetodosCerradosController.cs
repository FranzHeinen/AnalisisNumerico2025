using AnalisisNumerico_RaicesDeFunciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodosCerradosController : Controller
    {
        private readonly MetodosCerrados _service;

        public MetodosCerradosController (MetodosCerrados service, ILogger<MetodosCerradosController> logger)
        {
            _service = service;
        }

        [HttpPost("biseccion")]
        public ActionResult<ResultadoDTO> Biseccion([FromBody] MetodoCerradoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resultado = _service.Biseccion(request);
                return Ok(resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPost("regla-falsa")]
        public ActionResult<ResultadoDTO> ReglaFalsa([FromBody] MetodoCerradoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resultado = _service.ReglaFalsa(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
