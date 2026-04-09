using AnalisisNumerico_AjusteDeCurva;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegresionLinealController : Controller
    {
        private readonly RegresionLinealService _service;

        public RegresionLinealController(RegresionLinealService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<RegresionLinealResultado> Calcular([FromBody] RegresionLinealRequest request)
        {
            if (!ModelState.IsValid || request.Puntos == null || request.Puntos.Count < 2)
            {
                return BadRequest("La entrada no es válida. Se requieren al menos dos puntos.");
            }

            try
            {
                var resultado = _service.Calcular(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("recalcular")]
        public IActionResult RecalcularR2([FromBody] RegresionModificadaRequest request)
        {
            try
            {
                if (request == null || request.PuntosCargados == null || request.PuntosCargados.Count == 0)
                {
                    return BadRequest("No se proporcionaron puntos para el cálculo.");
                }

                if (string.IsNullOrWhiteSpace(request.FuncionModificada))
                {
                    return BadRequest("La función modificada no puede estar vacía.");
                }

                var nuevoR2 = _service.CalcularNuevoR2(request.PuntosCargados, request.FuncionModificada);
                return Ok(new { nuevoR2 = nuevoR2 });
            }
            catch (FormatException ex)
            {
                // Error al parsear la función (ej: "y = 2x mal")
                return BadRequest($"Error en el formato de la función: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Otro error
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

