using Calculus;
using System.Xml;
namespace AnalisisNumerico_RaicesDeFunciones
{
    public class ResultadoDTO
    {
        public double Xr {  get; set; }
        public int Iteraciones { get; set; }
        public double Error { get; set; }
        public bool Converge { get; set; }
        public string Funcion {  get; set; }
        public string Metodo {  get; set; }
    }
}