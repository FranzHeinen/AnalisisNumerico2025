using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_RaicesDeFunciones
{
    public class MetodoNewtonRapshonRequest
    {
        public string Funcion { get; set; }
        public double Xi { get; set; }
        public int MaxIteraciones { get; set; }
        public double Tolerancia { get; set; }
    }
}
