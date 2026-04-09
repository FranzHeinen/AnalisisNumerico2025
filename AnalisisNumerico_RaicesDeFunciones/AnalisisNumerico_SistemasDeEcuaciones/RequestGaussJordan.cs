using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_SistemasDeEcuaciones
{
    public class RequestGaussJordan
    {
        public double[][] A { get; set; }
        public double[] b { get; set; }
        public double Tolerancia { get; set; }
        public int Iteraciones { get; set; }
    }
}
