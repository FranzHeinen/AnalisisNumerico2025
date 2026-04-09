using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionLinealService
    {
        public RegresionLinealResultado Calcular(RegresionLinealRequest request)
        {
            var puntos = request.Puntos;
            var tolerancia = request.Tolerancia;

            int n = puntos.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;

            foreach (var punto in puntos)
            {
                double x = punto[0];
                double y = punto[1];
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
                sumY2 += y * y;
            }

            double a1 = (n * sumXY - sumX * sumY) / (n * sumX2 - (sumX * sumX));
            double a0 = (sumY - a1 * sumX) / n;

            double st = 0, sr = 0;
            foreach (var punto in puntos)
            {
                double x = punto[0];
                double y = punto[1];
                double yEstimada = a1 * x + a0;
                st += Math.Pow(y - (sumY / n), 2);
                sr += Math.Pow(y - yEstimada, 2);
            }

            double r = Math.Sqrt((st - sr) / st) * 100;

            return new RegresionLinealResultado
            {
                Funcion = $"y = {a1:F4}x {(a0 >= 0 ? "+" : "-")} {Math.Abs(a0):F4}",
                Correlacion = r,
                EfectividadAjuste = r >= tolerancia * 100
                    ? "El ajuste es aceptable"
                    : "El ajuste no es aceptable"
            };
        }

        private Tuple<double, double> ObtenerCoeficientesFuncion(string funcion)
        {
            if (string.IsNullOrWhiteSpace(funcion))
                throw new ArgumentException("La función no puede estar vacía.");

            // Regex para y = a1x + a0 (con espacios opcionales y +/-)
            var regex = new Regex(@"y\s*=\s*([+-]?\d+(?:[.,]\d+)?)x\s*([+-]\s*\d+(?:[.,]\d+)?)");
            var match = regex.Match(funcion);

            if (!match.Success)
                throw new FormatException("Formato inválido. Ejemplo esperado: y = 2.5x - 1.3");

            string a1Str = match.Groups[1].Value.Replace(',', '.');
            string a0Str = match.Groups[2].Value.Replace(',', '.').Replace(" ", "");

            double a1 = double.Parse(a1Str, CultureInfo.InvariantCulture);
            double a0 = double.Parse(a0Str, CultureInfo.InvariantCulture);

            return Tuple.Create(a1, a0);
        }

        public double CalcularNuevoR2(List<double[]> PuntosCargados, string funcion)
        {
            double sumY = 0;
            foreach (double[] punto in PuntosCargados)
            {
                sumY += punto[1]; // punto[1] es Y
            }

            var coeficientes = ObtenerCoeficientesFuncion(funcion);
            double a1 = coeficientes.Item1; // Pendiente
            double a0 = coeficientes.Item2; // Ordenada al origen

            double st = 0; // Suma total de los cuadrados
            double sr = 0; // Suma de los cuadrados de los residuos

            double promY = sumY / PuntosCargados.Count;

            foreach (double[] punto in PuntosCargados)
            {
                double y_real = punto[1];
                double x_valor = punto[0];
                double y_predicha = (a1 * x_valor) + a0;

                st += Math.Pow(y_real - promY, 2);
                sr += Math.Pow(y_real - y_predicha, 2);
            }

            // Este es el r^2 (Coeficiente de Determinación) como porcentaje
            // (st - sr) / st es r^2. Lo multiplicamos por 100 para el porcentaje.
            double r2 = ((st - sr) / st) * 100;

            return r2;
        }
    }
}
