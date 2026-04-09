using AnalisisNumerico_RaicesDeFunciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodosAbiertosController : Controller
    {
        private readonly MetodosAbiertos _service;

        public MetodosAbiertosController (MetodosAbiertos service)
        {
            _service = service;
        }

        [HttpPost("newton-raphson")]
        public ActionResult<ResultadoDTO> NewtonRaphson([FromBody] MetodoNewtonRapshonRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resultado = _service.NewtonRaphson(request);
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
        [HttpPost("secante")]
        public ActionResult<ResultadoDTO> Secante([FromBody] MetodoCerradoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resultado = _service.Secante(request);
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
    }
}
