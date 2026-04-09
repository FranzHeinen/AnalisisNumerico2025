using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionPolinomialResultado
    {
        public string Funcion { get; set; } = string.Empty;
        public double Correlacion { get; set; }             // r en %
        public string EfectividadAjuste { get; set; } = "";
        public double[] Coeficientes { get; set; } = System.Array.Empty<double>(); // [a0..ag]
    }
}
