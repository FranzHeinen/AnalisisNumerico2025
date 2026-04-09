using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionPolinomialRequest
    {
        public List<double[]> Puntos { get; set; } = new(); // [x,y]
        public int Grado { get; set; }                      // g >= 1
        public double Tolerancia { get; set; } = 0.8;       // r mínimo (0..1)
    }
}
