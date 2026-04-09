using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionModificadaRequest
    {
        public List<double[]> PuntosCargados { get; set; }
        public string FuncionModificada { get; set; }
    }
}
