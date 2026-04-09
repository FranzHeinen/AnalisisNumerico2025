namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionLinealRequest
    {
        public List<double[]> Puntos { get; set; } // cada elemento es un array de 2 elementos: [x, y]
        public double Tolerancia { get; set; } = 0.8;
    }
}
