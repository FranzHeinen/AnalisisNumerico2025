using AnalisisNumerico_IntegracionNuemrica;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL será 'api/integracion'
    public class IntegracionController : ControllerBase
    {
        private readonly IntegracionService _servicio = new IntegracionService();

        [HttpPost("trapecio-simple")] // La URL completa será 'api/integracion/trapecio-simple'
        public IActionResult CalcularTrapecioSimple([FromBody] TrapecioSimpleRequest request)
        {
            var respuesta = _servicio.CalcularIntegralTrapeciosSimple(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                // Si hay un error, devolvemos un 'Bad Request' (código 400)
                return BadRequest(respuesta);
            }
            // Si todo está bien, devolvemos un 'OK' (código 200)
            return Ok(respuesta);
        }

        [HttpPost("trapecio-multiple")] // URL: api/integracion/trapecio-multiple
        public IActionResult CalcularTrapecioMultiple([FromBody] TrapecioMultipleRequest request)
        {
            // Validamos que 'n' sea un número positivo
            if (request.N <= 0)
            {
                return BadRequest(new IntegracionResponse { Error = "La cantidad de subintervalos (n) debe ser mayor a 0." });
            }

            var respuesta = _servicio.CalcularIntegralTrapeciosMultiple(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPost("simpson13-simple")] // URL: api/integracion/simpson13-simple
        public IActionResult CalcularSimpson13Simple([FromBody] TrapecioSimpleRequest request)
        {
            var respuesta = _servicio.CalcularIntegralSimpson13Simple(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPost("simpson13-multiple")] // URL: api/integracion/simpson13-multiple
        public IActionResult CalcularSimpson13Multiple([FromBody] Simpson13MultipleRequest request)
        {
            // Regla de Simpson 1/3: 'n' debe ser un número par.
            if (request.N % 2 != 0)
            {
                return BadRequest(new IntegracionResponse { Error = "Para Simpson 1/3, 'n' debe ser un número par." });
            }

            var respuesta = _servicio.CalcularIntegralSimpson13Multiple(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPost("simpson38")] // URL: api/integracion/simpson38
        public IActionResult CalcularSimpson38([FromBody] TrapecioSimpleRequest request)
        {
            var respuesta = _servicio.CalcularIntegralSimpson38(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPost("simpson-combinado")] // URL: api/integracion/simpson-combinado
        public IActionResult CalcularSimpsonCombinado([FromBody] Simpson13MultipleRequest request)
        {
            if (request.N <= 0)
            {
                return BadRequest(new IntegracionResponse { Error = "La cantidad de subintervalos (n) debe ser mayor a 0." });
            }

            // La lógica del servicio ya maneja si N es par o impar.
            var respuesta = _servicio.CalcularIntegralSimpsonCombinado(request);

            if (!string.IsNullOrEmpty(respuesta.Error))
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }
    }
}
