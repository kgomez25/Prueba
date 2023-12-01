using System;
using System.Collections.Generic;
using System.Linq;

namespace citas
{
    internal class Program
    {
        class Cliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Telefono { get; set; }
        }

        class Cita
        {
            public int Id { get; set; }
            public DateTime FechaHora { get; set; }
            public Cliente Cliente { get; set; }
            public string Servicio { get; set; }
        }

        class Program1
        {
            static List<Cliente> clientes = new List<Cliente>();
            static List<Cita> citas = new List<Cita>();

            static void Main()
            {
                while (true)
                {
                    MostrarMenu();
                    int opcion = ObtenerOpcionUsuario();

                    EjecutarOpcion(opcion);
                }
            }

            static void MostrarMenu()
            {
                Console.WriteLine();
                Console.WriteLine("****************************** Sistema de citas para peluquería ******************************");
                Console.WriteLine();
                Console.WriteLine("Menú:");
                Console.WriteLine();
                Console.WriteLine("1. Agregar Cliente");
                Console.WriteLine("2. Programar Cita");
                Console.WriteLine("3. Mostrar Citas");
                Console.WriteLine("4. Cancelar Cita");
                Console.WriteLine("5. Salir");
            }

            static int ObtenerOpcionUsuario()
            {
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");
                return Convert.ToInt32(Console.ReadLine());
            }

            static void EjecutarOpcion(int opcion)
            {
                switch (opcion)
                {
                    case 1:
                        AgregarCliente();
                        break;
                    case 2:
                        ProgramarCita();
                        break;
                    case 3:
                        MostrarCitas();
                        break;
                    case 4:
                        CancelarCita();
                        break;
                    case 5:
                        Salir();
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }
            }

            static void AgregarCliente()
            {
                Console.Clear();
                Console.WriteLine("Ingrese el nombre del cliente:");
                string nombre = SafeReadLine();

                // Validación para el número de teléfono
                string telefono;
                do
                {
                    Console.WriteLine("Ingrese el número de teléfono del cliente (solo números y debe tener 10 dígitos):");
                    telefono = SafeReadLine();
                } while (!EsNumero(telefono) || telefono.Length != 10);

                Cliente cliente = new Cliente
                {
                    Id = clientes.Count + 1,
                    Nombre = nombre,
                    Telefono = telefono
                };

                clientes.Add(cliente);
                Console.WriteLine($"Cliente {cliente.Nombre} agregado con éxito.");
                Console.Clear();
            }

            // Método para verificar si una cadena es un número
            static bool EsNumero(string input)
            {
                return input.All(char.IsDigit);
            }

            static void ProgramarCita()
            {
                if (clientes.Count == 0)
                {
                    Console.WriteLine("No hay clientes registrados. Por favor, agregue un cliente primero.");
                    return;
                }

                Console.Clear();
                Console.WriteLine("Seleccione el cliente para la cita:");
                MostrarClientes();

                int idCliente = Convert.ToInt32(SafeReadLine());
                Cliente cliente = ObtenerClientePorId(idCliente);

                Console.Clear();
                Console.WriteLine("Ingrese la fecha y hora de la cita (yyyy-MM-dd HH:mm):");
                DateTime fechaHora;
                while (!DateTime.TryParseExact(SafeReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out fechaHora))
                {
                    Console.WriteLine("Formato de fecha y hora incorrecto. Intente nuevamente.");
                }

                Console.WriteLine("Ingrese el servicio de la cita:");
                string servicio = SafeReadLine();

                Cita cita = new Cita
                {
                    Id = citas.Count + 1,
                    FechaHora = fechaHora,
                    Cliente = cliente,
                    Servicio = servicio
                };
                Console.Clear();
                citas.Add(cita);
                Console.WriteLine($"Cita programada con éxito para {cliente.Nombre} el {fechaHora} para el servicio de {servicio}.");
            }

            static void MostrarClientes()
            {
                Console.Clear();
                Console.WriteLine("Lista de Clientes:");
                foreach (var cliente in clientes)
                {
                    Console.WriteLine($"ID: {cliente.Id}, Nombre: {cliente.Nombre}, Teléfono: {cliente.Telefono}");
                }
            }

            static void MostrarCitas()
            {
                Console.Clear();
                if (citas.Count == 0)
                {
                    Console.WriteLine("No hay citas programadas actualmente.");
                    return;
                }

                Console.WriteLine("Lista de Citas:");
                foreach (var cita in citas)
                {
                    Console.WriteLine($"ID: {cita.Id}, Cliente: {cita.Cliente.Nombre}, Fecha y Hora: {cita.FechaHora}, Servicio: {cita.Servicio}");
                }
            }

            static void CancelarCita()
            {
                Console.Clear();
                if (citas.Count == 0)
                {
                    Console.WriteLine("No hay citas programadas para cancelar.");
                    return;
                }

                Console.WriteLine("Ingrese el ID de la cita que desea cancelar:");
                int idCita;
                while (!int.TryParse(SafeReadLine(), out idCita))
                {
                    Console.WriteLine("Ingrese un número válido.");
                }

                Cita cita = ObtenerCitaPorId(idCita);

                if (cita != null)
                {
                    citas.Remove(cita);
                    Console.WriteLine($"Cita para {cita.Cliente.Nombre} el {cita.FechaHora} para el servicio de {cita.Servicio} ha sido cancelada.");
                }
                else
                {
                    Console.WriteLine("No se encontró ninguna cita con el ID proporcionado.");
                }
                Console.Clear();
            }

            static Cliente ObtenerClientePorId(int id)
            {
                return clientes.Find(c => c.Id == id);
            }

            static Cita ObtenerCitaPorId(int id)
            {
                return citas.Find(c => c.Id == id);
            }

            static void Salir()
            {
                Console.WriteLine("Saliendo del programa. ¡Hasta luego!");
                Environment.Exit(0);
            }

            // Bloquea la tecla Enter si no se ha ingresado nada
            static string SafeReadLine()
            {
                string input = "";
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            Console.WriteLine(); // Imprime una nueva línea para mantener el formato
                            return input;
                        }
                        else
                        {
                            Console.WriteLine("Por favor, ingrese un valor.");
                        }
                    }
                    else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b"); // Borra el carácter y mueve el cursor hacia atrás
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
            }
        }
    }
}
