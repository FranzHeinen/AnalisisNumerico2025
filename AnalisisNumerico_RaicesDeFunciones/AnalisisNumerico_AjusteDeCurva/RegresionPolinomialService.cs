using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Linq;
using System.Collections.Generic;
using AnalisisNumerico_SistemasDeEcuaciones; // <— TU GaussJordan

namespace AnalisisNumerico_AjusteDeCurva
{
    public class RegresionPolinomialService
    {
        public RegresionPolinomialResultado Calcular(RegresionPolinomialRequest request)
        {
            var puntos = request.Puntos;
            int g = request.Grado;
            double tol = request.Tolerancia;

            if (g < 1) throw new ArgumentException("El grado debe ser ≥ 1.");
            if (puntos == null || puntos.Count < g + 1)
                throw new ArgumentException($"Se requieren al menos {g + 1} puntos para grado {g}.");

            int n = puntos.Count;
            int m = g + 1;
            int maxPow = 2 * g;

            // ===== Ecuaciones normales =====
            double[] S = new double[maxPow + 1]; // Σ x^k
            double[] b = new double[m];          // Σ y x^i

            foreach (var p in puntos)
            {
                double x = p[0], y = p[1];
                for (int k = 0; k <= maxPow; k++) S[k] += Math.Pow(x, k);
                for (int i = 0; i <= g; i++) b[i] += y * Math.Pow(x, i);
            }

            // A[i,j] = S[i+j]  (jagged para tu RequestGaussJordan)
            double[][] A = new double[m][];
            for (int i = 0; i < m; i++)
            {
                A[i] = new double[m];
                for (int j = 0; j < m; j++) A[i][j] = S[i + j];
            }

            // ===== Resolver con TU Gauss-Jordan =====
            var sistema = new RequestGaussJordan { A = A, b = b };
            double[] coef = GaussJordan.Resolver(sistema); // [a0..ag]

            // ===== r% =====
            double promY = puntos.Average(pt => pt[1]);
            double st = 0, sr = 0;
            foreach (var p in puntos)
            {
                double x = p[0], y = p[1];
                double yHat = Evaluar(coef, x);
                st += Math.Pow(y - promY, 2);
                sr += Math.Pow(y - yHat, 2);
            }
            double rPct = (st > 1e-12) ? Math.Sqrt((st - sr) / st) * 100.0 : 0.0;

            return new RegresionPolinomialResultado
            {
                Funcion = Formatear(coef),
                Correlacion = Math.Round(rPct, 4),
                EfectividadAjuste = rPct >= tol * 100
                    ? "El ajuste es aceptable"
                    : "El ajuste no es aceptable",
                Coeficientes = coef
            };
        }

        private static double Evaluar(double[] c, double x)
        {
            double y = 0;
            for (int i = 0; i < c.Length; i++) y += c[i] * Math.Pow(x, i);
            return y;
        }

        private static string Formatear(double[] c)
        {
            string s = "y = ";
            bool first = true;
            for (int i = c.Length - 1; i >= 0; i--)
            {
                double a = Math.Round(c[i], 4);
                if (Math.Abs(a) < 1e-12) continue;
                string sign = first ? (a < 0 ? "-" : "") : (a < 0 ? " - " : " + ");
                string term = i == 0 ? $"{Math.Abs(a):F4}"
                            : i == 1 ? $"{Math.Abs(a):F4}x"
                                     : $"{Math.Abs(a):F4}x^{i}";
                s += sign + term;
                first = false;
            }
            if (first) s += "0";
            return s;
        }
    }
}

