namespace AnalisisNumerico_SistemasDeEcuaciones
{
    public class GaussSeidel
    {
        public static double[] Resolver(RequestGaussJordan request)
        {
            int n = request.A.Length;
            double[,] matriz = new double[n, n + 1];

            // Paso 0: Construir matriz aumentada [A | b]
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    matriz[i, j] = request.A[i][j];

                matriz[i, n] = request.b[i];
            }

            double tolerancia = request.Tolerancia;
            int maxIter = request.Iteraciones;
            int contador = 0;
            bool esSolucion = false;

            double[] vectorResultado = new double[n];       // Inicializado en 0
            double[] vectorAnterior = new double[n];

            while (contador < maxIter && !esSolucion)
            {
                contador++;
                Array.Copy(vectorResultado, vectorAnterior, n);  // Guardamos la iteración anterior

                for (int row = 0; row < n; row++)
                {
                    double suma = matriz[row, n];  // Comenzamos con el término independiente b

                    for (int col = 0; col < n; col++)
                    {
                        if (col != row)
                        {
                            suma -= matriz[row, col] * vectorResultado[col];  // Usa los valores actualizados
                        }
                    }

                    double coefDiag = matriz[row, row];
                    if (coefDiag == 0)
                        throw new Exception($"Pivote nulo en diagonal principal");

                    vectorResultado[row] = suma / coefDiag;
                }

                // Verificar convergencia
                int coincidencias = 0;
                for (int i = 0; i < n; i++)
                {
                    double errorRelativo = Math.Abs((vectorResultado[i] - vectorAnterior[i]) / vectorResultado[i]);
                    if (errorRelativo < tolerancia)
                        coincidencias++;
                }

                esSolucion = coincidencias == n;
            }

            if (!esSolucion)
                throw new Exception("Se superó el número máximo de iteraciones sin converger.");

            return vectorResultado;
        }
    }
}