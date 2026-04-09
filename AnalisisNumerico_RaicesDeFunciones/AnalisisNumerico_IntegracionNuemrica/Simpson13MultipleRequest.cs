using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_IntegracionNuemrica
{
    public class Simpson13MultipleRequest
    {
        public string Funcion { get; set; }
        public double Xi { get; set; }
        public double Xd { get; set; }
        public int N { get; set; }
    }
}
