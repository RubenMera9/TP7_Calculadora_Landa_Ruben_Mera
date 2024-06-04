using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Calculadora
{
    class Program
    {
        // Lista para almacenar los registros de operaciones
        static List<RecordOperaciones> registros = new List<RecordOperaciones>();

        static void Main(string[] args)
        {
            do
            {
                // Preguntar al usuario qué tipo de números desea utilizar
                Console.WriteLine("¿Desea trabajar con enteros (1), decimales (2), o salir (3)?");
                int tipoNumero = Convert.ToInt32(Console.ReadLine());

                if (tipoNumero == 3)
                {
                    // Si elige salir, mostrar el menú y terminar el programa
                    MostrarMenu();
                    return;
                }

                bool trabajarConDecimales = tipoNumero == 2;

                // Realizar operaciones con los números seleccionados
                RealizarOperaciones(trabajarConDecimales);

            } while (true);
        }

        static void MostrarMenu()
        {
            // Mostrar el menú de opciones
            Console.WriteLine("----- MENÚ -----");
            Console.WriteLine("1 - Mostrar registros de operaciones");
            Console.WriteLine("2 - Salir");
            Console.WriteLine("----------------");
            int opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    // Mostrar los registros de operaciones
                    MostrarRegistros();
                    break;
                case 2:
                    // Salir del programa
                    Console.WriteLine("Presiona cualquier tecla para salir...");
                    Console.ReadKey();
                    return;
                default:
                    // Opción inválida
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }

        static void RealizarOperaciones(bool trabajarConDecimales)
        {
            do
            {
                Console.WriteLine("---------------CALCULADORA------------");
                Console.WriteLine("Introduce el primer número:");
                // Leer el primer número
                double a = LeerNumero(trabajarConDecimales);

                Console.WriteLine("Introduzca el segundo número: ");
                // Leer el segundo número
                double b = LeerNumero(trabajarConDecimales);

                // Verificar si uno de los números no es válido
                if (double.IsNaN(a) || double.IsNaN(b))
                {
                    Console.WriteLine("Error: No se pudo leer uno o ambos números. Inténtalo nuevamente.");
                    continue;
                }

                // Mostrar las opciones de operación
                Console.WriteLine("Elija una opción:");
                Console.WriteLine("1- Sumar\n2 - Restar \n3 - Dividir \n4 - Multiplicar\n5 - Cambiar tipo de número\n6 - Salir ");
                int opcion = Convert.ToInt32(Console.ReadLine());

                double resultado = 0;

                string nombreOperacion = "";

                switch (opcion)
                {
                    case 1:
                        // Realizar la operación de suma
                        resultado = Operacion((x, y) => x + y, a, b);
                        nombreOperacion = "Suma";
                        break;
                    case 2:
                        // Realizar la operación de resta
                        resultado = Operacion((x, y) => x - y, a, b);
                        nombreOperacion = "Resta";
                        break;
                    case 3:
                        // Realizar la operación de división
                        if (b == 0)
                        {
                            Console.WriteLine("La división no es válida");
                            nombreOperacion = "División";
                            resultado = double.NaN; // Marcar resultado como NaN para indicar error
                        }
                        else
                        {
                            resultado = Operacion((x, y) => x / y, a, b);
                            nombreOperacion = "División";
                        }
                        break;
                    case 4:
                        // Realizar la operación de multiplicación
                        resultado = Operacion((x, y) => x * y, a, b);
                        nombreOperacion = "Multiplicación";
                        break;
                    case 5:
                        // Cambiar el tipo de número
                        trabajarConDecimales = !trabajarConDecimales;
                        continue;
                    case 6:
                        // Salir de la función
                        return;
                    default:
                        // Opción inválida
                        Console.WriteLine("Opción inválida.");
                        continue;
                }

                // Registrar la operación en el registro de operaciones
                registros.Add(new RecordOperaciones() { Operador1 = a, Operador2 = b, Operador = nombreOperacion, Resultado = resultado });

                // Imprimir el resultado, incluyendo errores
                if (double.IsNaN(resultado))
                {
                    Console.WriteLine($"Operación: {nombreOperacion} | Operador1: {a} | Operador2: {b} | Resultado: Error");
                }
                else
                {
                    Console.WriteLine($"Operación: {nombreOperacion} | Operador1: {a} | Operador2: {b} | Resultado: {resultado}");
                }

                Console.WriteLine("Desea continuar: 1=si/0=no");
                int continuar = Convert.ToInt32(Console.ReadLine());
                if (continuar == 0)
                {
                    MostrarRegistros();
                    return;
                }

            } while (true);
        }

        static double LeerNumero(bool trabajarConDecimales)
        {
            // Leer el número del usuario
            string input = Console.ReadLine();
            double numero;
            bool esNumero = double.TryParse(input, out numero);

            if (!esNumero)
            {
                Console.WriteLine("Error: Debe ingresar un número.");
                return double.NaN;
            }

            if (!trabajarConDecimales)
            {
                if (!EsNumeroEntero(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número entero.");
                    return double.NaN;
                }
                return Convert.ToInt32(input);
            }
            else
            {
                if (!EsNumeroDecimal(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número decimal.");
                    return double.NaN;
                }
                return Convert.ToDouble(input, CultureInfo.InvariantCulture);
            }
        }

        private static bool EsNumeroEntero(string input)
        {
            // Verificar si la cadena es un número entero
            int result;
            return int.TryParse(input, out result);
        }

        private static bool EsNumeroDecimal(string input)
        {
            // Verificar si la cadena es un número decimal
            double result;
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        private static double Operacion(Func<double, double, double> operacion, double a, double b)
        {
            // Realizar la operación utilizando una función de delegado
            return operacion(a, b);
        }

        static void MostrarRegistros()
        {
            // Mostrar los registros de operaciones
            Console.WriteLine("Registros de Operaciones:");
            foreach (var registro in registros)
            {
                if (double.IsNaN(registro.Resultado))
                {
                    // Si el resultado es un NaN, se trata de un error
                    Console.WriteLine($"Operación: {registro.Operador} | Operador1: {registro.Operador1} | Operador2: {registro.Operador2} | Resultado: Error");
                }
                else
                {
                    // Si no hay error, se imprime el resultado normalmente
                    Console.WriteLine($"Operación: {registro.Operador} | Operador1: {registro.Operador1} | Operador2: {registro.Operador2} | Resultado: {registro.Resultado}");
                }
            }

            // Esperar a que el usuario presione una tecla para volver al menú
            Console.WriteLine("Presiona cualquier tecla para volver al menú...");
            Console.ReadKey();
            MostrarMenu();
        }
    }

    // Clase para representar los registros de operaciones
    class RecordOperaciones
    {
        public double Operador1 { get; set; }
        public double Operador2 { get; set; }
        public string Operador { get; set; }
        public double Resultado { get; set; }
    }
}
