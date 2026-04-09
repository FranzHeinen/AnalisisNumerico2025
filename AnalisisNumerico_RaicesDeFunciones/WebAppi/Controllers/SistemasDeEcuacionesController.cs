using AnalisisNumerico_SistemasDeEcuaciones;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("api/sistemas")]
    public class SistemasEcuacionesController : ControllerBase
    {
        [HttpPost("gaussjordan")]
        public ActionResult<double[]> ResolverGaussJordan([FromBody] RequestGaussJordan request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = GaussJordan.Resolver(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("gaussseidel")]
        public ActionResult<double[]> ResolverGaussSeidel([FromBody] RequestGaussJordan request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = GaussSeidel.Resolver(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

