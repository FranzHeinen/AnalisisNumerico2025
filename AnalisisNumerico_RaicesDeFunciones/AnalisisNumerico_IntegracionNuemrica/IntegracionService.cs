using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculus;

namespace AnalisisNumerico_IntegracionNuemrica
{
    public class IntegracionService
    {
        public IntegracionResponse CalcularIntegralTrapeciosSimple(TrapecioSimpleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();


            if (funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {

                double f_xi = funcionCalculus.EvaluaFx(request.Xi);
                double f_xd = funcionCalculus.EvaluaFx(request.Xd);

                respuesta.Area = ((f_xi + f_xd) * (request.Xd - request.Xi)) / 2;
            }
            else
            {
                respuesta.Error = "Función mal ingresada";
            }
            return respuesta;
        }

        public IntegracionResponse CalcularIntegralTrapeciosMultiple(TrapecioMultipleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();

            // 1. Validar la sintaxis de la función
            if (funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {
                // 2. Calcular 'h' (ancho de cada subintervalo)
                double h = (request.Xd - request.Xi) / request.N;
                double sumatoria = 0;

                // 3. Bucle para la sumatoria desde i=1 hasta n-1
                for (int i = 1; i < request.N; i++)
                {
                    sumatoria += funcionCalculus.EvaluaFx(request.Xi + i * h);
                }

                // 4. Aplicar la fórmula completa del PDF
                double f_xi = funcionCalculus.EvaluaFx(request.Xi);
                double f_xd = funcionCalculus.EvaluaFx(request.Xd);

                respuesta.Area = (h / 2) * (f_xi + 2 * sumatoria + f_xd);
            }
            else
            {
                respuesta.Error = "Función mal ingresada";
            }
            return respuesta;
        }

        public IntegracionResponse CalcularIntegralSimpson13Simple(TrapecioSimpleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();

            if (funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {
                // 1. Según la fórmula, 'h' es la mitad del intervalo total
                double h = (request.Xd - request.Xi) / 2;

                // 2. Evaluamos la función en los 3 puntos clave: inicio, medio y final
                double f_xi = funcionCalculus.EvaluaFx(request.Xi);
                double f_x_medio = funcionCalculus.EvaluaFx(request.Xi + h);
                double f_xd = funcionCalculus.EvaluaFx(request.Xd);

                // 3. Aplicamos la fórmula de Simpson 1/3 Simple del PDF
                respuesta.Area = (h / 3) * (f_xi + 4 * f_x_medio + f_xd);
            }
            else
            {
                respuesta.Error = "Función mal ingresada";
            }
            return respuesta;
        }

        public IntegracionResponse CalcularIntegralSimpson13Multiple(Simpson13MultipleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();

            if (funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {
                double h = (request.Xd - request.Xi) / request.N;
                double sumatoriaPares = 0;
                double sumatoriaImpares = 0;

                // Bucle de 1 a n-1 para las sumatorias
                for (int i = 1; i < request.N; i++)
                {
                    if (i % 2 == 0) // Si 'i' es par
                    {
                        sumatoriaPares += funcionCalculus.EvaluaFx(request.Xi + i * h);
                    }
                    else // Si 'i' es impar
                    {
                        sumatoriaImpares += funcionCalculus.EvaluaFx(request.Xi + i * h);
                    }
                }

                double f_xi = funcionCalculus.EvaluaFx(request.Xi);
                double f_xd = funcionCalculus.EvaluaFx(request.Xd);

                // Aplicamos la fórmula completa de Simpson 1/3 Múltiple del PDF
                respuesta.Area = (h / 3) * (f_xi + 4 * sumatoriaImpares + 2 * sumatoriaPares + f_xd);
            }
            else
            {
                respuesta.Error = "Función mal ingresada";
            }
            return respuesta;
        }


        public IntegracionResponse CalcularIntegralSimpson38(TrapecioSimpleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();

            if (funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {
                // 1. Según la fórmula, 'h' se calcula dividiendo el intervalo en 3
                double h = (request.Xd - request.Xi) / 3;

                // 2. Evaluamos la función en los 4 puntos clave
                double f_x0 = funcionCalculus.EvaluaFx(request.Xi);         // f(a)
                double f_x1 = funcionCalculus.EvaluaFx(request.Xi + h);     // f(a+h)
                double f_x2 = funcionCalculus.EvaluaFx(request.Xi + 2 * h); // f(a+2h)
                double f_x3 = funcionCalculus.EvaluaFx(request.Xd);         // f(b)

                // 3. Aplicamos la fórmula de Simpson 3/8 del PDF
                respuesta.Area = (3 * h / 8) * (f_x0 + 3 * f_x1 + 3 * f_x2 + f_x3);
            }
            else
            {
                respuesta.Error = "Función mal ingresada";
            }
            return respuesta;
        }

        public IntegracionResponse CalcularIntegralSimpsonCombinado(Simpson13MultipleRequest request)
        {
            var respuesta = new IntegracionResponse();
            Calculo funcionCalculus = new Calculo();

            if (!funcionCalculus.Sintaxis(request.Funcion, 'x'))
            {
                respuesta.Error = "Función mal ingresada";
                return respuesta;
            }

            // Si 'n' es PAR, simplemente usamos Simpson 1/3 Múltiple que ya tenemos.
            if (request.N % 2 == 0)
            {
                return CalcularIntegralSimpson13Multiple(request);
            }

            // Si 'n' es IMPAR, aplicamos la lógica combinada del PDF.
            double resultadoTotal = 0;
            int n = request.N;
            double xd = request.Xd;

            // Caso especial: si n=3, es solo Simpson 3/8.
            if (n == 3)
            {
                var requestSimple = new TrapecioSimpleRequest { Funcion = request.Funcion, Xi = request.Xi, Xd = request.Xd };
                return CalcularIntegralSimpson38(requestSimple);
            }

            // Si 'n' es impar y mayor que 3...
            // 1. Calculamos el área de los ÚLTIMOS TRES intervalos con Simpson 3/8.
            double h = (request.Xd - request.Xi) / n;
            double nuevoXi = request.Xi + (n - 3) * h; // El 'xi' para la parte de 3/8.

            var request38 = new TrapecioSimpleRequest { Funcion = request.Funcion, Xi = nuevoXi, Xd = xd };
            double resultado38 = CalcularIntegralSimpson38(request38).Area.Value;
            resultadoTotal += resultado38;

            // 2. Ajustamos 'n' y 'xd' para el resto del cálculo, que ahora tiene un número PAR de intervalos.
            n = n - 3;
            xd = nuevoXi;

            // 3. Calculamos el área restante con Simpson 1/3 Múltiple.
            var request13Multiple = new Simpson13MultipleRequest { Funcion = request.Funcion, Xi = request.Xi, Xd = xd, N = n };
            double resultado13Multiple = CalcularIntegralSimpson13Multiple(request13Multiple).Area.Value;

            resultadoTotal += resultado13Multiple;

            respuesta.Area = resultadoTotal;
            return respuesta;
        }
    }
}
