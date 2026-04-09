using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisNumerico_SistemasDeEcuaciones
{
    public static class GaussJordan
    {
        public static double[] Resolver (RequestGaussJordan request)
        {
            int dimension = request.A.Length;

            // Paso 0: Crear la matriz aumentada [A | b]
            double[,] matriz = new double[dimension, dimension + 1];

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                    matriz[i, j] = request.A[i][j]; // Cargamos A en la parte izquierda

                matriz[i, dimension] = request.b[i]; // Cargamos b en la última columna
            }

            // PASO 1: Recorremos cada fila-pivote (diagonal principal)
            for (int rowDiag = 0; rowDiag < dimension; rowDiag++)
            {
                // Paso 2.a: Tomar el coeficiente de la diagonal principal
                double coefDiag = matriz[rowDiag, rowDiag];
                if (coefDiag == 0)
                    throw new Exception($"División por cero en diagonal principal (pivote nulo)");

                // Paso 2.b: Normalizar toda la fila dividiendo por el coeficiente de la diagonal
                for (int col = 0; col < dimension + 1; col++)
                    matriz[rowDiag, col] /= coefDiag;

                // Paso 2.c: Hacer ceros en la columna actual para todas las demás filas
                for (int row = 0; row < dimension; row++)
                {
                    if (row == rowDiag) continue; // Saltamos la fila pivote

                    // Paso 2.d: Tomar el coeficiente que queremos eliminar
                    double coefCero = matriz[row, rowDiag];

                    // Paso 2.e: Restamos un múltiplo de la fila pivote para hacer cero ese valor
                    for (int col = 0; col < dimension + 1; col++)
                        matriz[row, col] -= coefCero * matriz[rowDiag, col];
                }
            }

            // PASO 3: Extraer el vector solución desde la última columna
            double[] resultado = new double[dimension];
            for (int i = 0; i < dimension; i++)
                resultado[i] = matriz[i, dimension];

            return resultado;
        }
    }
}
