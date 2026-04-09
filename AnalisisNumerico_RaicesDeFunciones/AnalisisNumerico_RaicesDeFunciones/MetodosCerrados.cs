using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculus;

namespace AnalisisNumerico_RaicesDeFunciones
{
    public class MetodosCerrados
    {
        public ResultadoDTO Biseccion (MetodoCerradoRequest request)
        {
            var result = new ResultadoDTO { Metodo = "Biseccion"};
            Calculo calculo = new Calculo();

            if (!calculo.Sintaxis(request.Funcion, 'x'))
            {
                throw new ArgumentException("Error en la sintaxis de la funcion");
            }

            result.Funcion = request.Funcion;

            double fxi = calculo.EvaluaFx(request.Xi);
            double fxd = calculo.EvaluaFx(request.Xd);

                if (fxi * fxd > 0)
            {
                throw new ArgumentException("No hay cambio de signo en el intervalo dado.");
            }

            if (fxi * fxd == 0)
            {
                result.Iteraciones = 1;
                result.Error = 1;
                result.Converge = true;
                if (fxi == 0)
                {
                    result.Xr = request.Xi;   
                }else
                {
                    result.Xr = request.Xd;
                }
                return result;
            }else 
            {
                double xi = request.Xi;
                double xd = request.Xd;
                double xrAnterior = 0; //pregutar a profe
                double xr = 0;
                double error = 0;

                for (int i = 1; i <= request.MaxIteraciones; i++)
                {
                    fxi = calculo.EvaluaFx(request.Xi);
                    fxd = calculo.EvaluaFx(request.Xd);

                    xr = 0.5 * (xi + xd);
                    error = Math.Abs((xr - xrAnterior)/ xr);
                    double fxr = calculo.EvaluaFx(xr);

                    if (Math.Abs(fxr) < request.Tolerancia ||i>request.MaxIteraciones || (error < request.Tolerancia)) // ver
                    {
                        result.Xr = xr;
                        result.Iteraciones = i;
                        result.Error = error;
                        result.Converge = true;
                        return result;
                    }
                    else
                    {
                        if (fxi*fxr > 0)
                        {
                            xi = xr;
                        }
                        else
                        {
                            xd = xr;
                        }
                        xrAnterior = xr;
                    }
                }
                result.Xr = xr;
                result.Iteraciones = request.MaxIteraciones;
                result.Error = error;
                result.Converge = false;
                return result;
            }
        }
        public ResultadoDTO ReglaFalsa(MetodoCerradoRequest request)
        {
            var result = new ResultadoDTO { Metodo = "Regla falsa" };
            Calculo calculo = new Calculo();

            if (!calculo.Sintaxis(request.Funcion, 'x'))
            {
                throw new ArgumentException("Error en la sintaxis de la funcion");
            }

            result.Funcion = request.Funcion;

            double fxi = calculo.EvaluaFx(request.Xi);
            double fxd = calculo.EvaluaFx(request.Xd);

            if (fxi * fxd > 0)
            {
                throw new ArgumentException("No hay cambio de signo en el intervalo dado.");
            }

            if (fxi * fxd == 0)
            {
                result.Iteraciones = 1;
                result.Error = 1;
                result.Converge = true;
                if (fxi == 0)
                {
                    result.Xr = request.Xi;
                }
                else
                {
                    result.Xr = request.Xd;
                }
                return result;
            }
            else
            {
                double xi = request.Xi;
                double xd = request.Xd;
                double xrAnterior = 0; //pregutar a profe
                double xr = 0;
                double error = 0;

                for (int i = 1; i <= request.MaxIteraciones; i++)
                {
                    fxi = calculo.EvaluaFx(xi);
                    fxd = calculo.EvaluaFx(xd);

                    xr = (xi * fxd - xd * fxi) / (fxd - fxi);
                    error = Math.Abs((xr - xrAnterior) / xr);
                    double fxr = calculo.EvaluaFx(xr);

                    if (Math.Abs(fxr) < request.Tolerancia || i > request.MaxIteraciones || (error < request.Tolerancia))
                    {
                        result.Xr = xr;
                        result.Iteraciones = i;
                        result.Error = error;
                        result.Converge = true;
                        return result;
                    }
                    else
                    {
                        if (fxi * fxr > 0)
                        {
                            xi = xr;
                        }
                        else
                        {
                            xd = xr;
                        }
                        xrAnterior = xr;
                    }
                }
                result.Xr = xr;
                result.Iteraciones = request.MaxIteraciones;
                result.Error = error;
                result.Converge = false;
                return result;
            }
        }
    }
}
