using System;
using System.Globalization;

namespace AnalisisNumerico_RaicesDeFunciones
{
    class Program
    {
        static void Main()
        {
            // 1) Datos generales
            Console.WriteLine("=== Prueba de Métodos Cerrados ===\n");
            Console.Write("Ingresa la función f(x): ");
            string fx = Console.ReadLine();

            Console.Write("Ingresa número máximo de iteraciones: ");
            int maxIter = int.Parse(Console.ReadLine());

            //Este formato para que corran las iteraciones
            Console.Write("Ingresa tolerancia: ");
            double tol = double.Parse(Console.ReadLine());

            // 2) Ejecutar Bisección
            Console.WriteLine("\n--- Bisección ---");
            var resBis = MetodosCerrados.Biseccion(fx, maxIter, tol);
            MostrarResultado(resBis);

            // 3) Ejecutar Regla Falsa
            Console.WriteLine("\n--- Regla Falsa ---");
            var resRF = MetodosCerrados.ReglaFalsa(fx, maxIter, tol);
            MostrarResultado(resRF);

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static void MostrarResultado(MetodoCerradoResultado r)
        {
            Console.WriteLine($"Función       : {r.Funcion}");
            Console.WriteLine($"Método        : {r.Metodo}");
            Console.WriteLine($"Raíz (Xr)     : {r.Xr:G9}");
            Console.WriteLine($"Iteraciones   : {r.Iteraciones}");
            Console.WriteLine($"Error relativo: {r.Error:E2}");
            Console.WriteLine($"Convergió     : {(r.Converge ? "Sí" : "No")}\n");
        }
    }
}