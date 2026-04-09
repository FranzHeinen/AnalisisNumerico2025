using AnalisisNumerico_AjusteDeCurva;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/RegresionPolinomial
    public class RegresionPolinomialController : ControllerBase
    {
        private readonly RegresionPolinomialService _svc;
        public RegresionPolinomialController(RegresionPolinomialService svc) => _svc = svc;

        [HttpPost]
        public ActionResult<RegresionPolinomialResultado> Calcular([FromBody] RegresionPolinomialRequest req)
        {
            if (req?.Puntos == null || req.Puntos.Count < req.Grado + 1)
                return BadRequest($"Se requieren al menos {req.Grado + 1} puntos.");
            if (req.Grado < 1) return BadRequest("El grado debe ser ≥ 1.");

            try { return Ok(_svc.Calcular(req)); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }
    }
}
