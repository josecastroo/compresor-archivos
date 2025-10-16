using System;
using System.IO;

namespace CompresorArchivos
{
    public class View
    {
        private readonly CompresorManager _manager;

        public View()
        {
            _manager = new CompresorManager();
        }

        public void Iniciar()
        {
            while (true)
            {
                Console.WriteLine("\n\tMenú");
                Console.WriteLine("\t1. Comprimir archivo");
                Console.WriteLine("\t2. Descomprimir archivo");
                Console.WriteLine("\t3. Salir");
                Console.Write("\tSeleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Comprimir();
                        break;
                    case "2":
                        Descomprimir();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
        }

        private void Comprimir()
        {
            Console.Write("Ingrese la ruta del archivo a comprimir: ");
            string archivo = Console.ReadLine();

            if (!File.Exists(archivo))
            {
                Console.WriteLine("El archivo no existe.");
                return;
            }

            Console.Write("Ingrese la ruta del archivo de salida (comprimido): ");
            string archivoBin = Console.ReadLine();

            try
            {
                var (textoComprimido, _, frequencies) = _manager.ComprimirArchivo(archivo);
                _manager.GuardarArchivoComprimido(archivoBin, textoComprimido, frequencies);
                Console.WriteLine("Archivo comprimido exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al comprimir: {ex.Message}");
            }
        }

        private void Descomprimir()
        {
            Console.Write("Ingrese la ruta del archivo a descomprimir: ");
            string archivo = Console.ReadLine();

            if (!File.Exists(archivo))
            {
                Console.WriteLine("El archivo no existe.");
                return;
            }

            Console.Write("Ingrese la ruta del archivo de salida (descomprimido): ");
            string archivoBin = Console.ReadLine();

            try
            {
                var (textoComprimido, HuffmanTree) = _manager.CargarArchivoComprimido(archivo);
                string textoDescomprimido = _manager.DescomprimirArchivo(textoComprimido, HuffmanTree);
                File.WriteAllText(archivoBin, textoDescomprimido);
                Console.WriteLine("Archivo descomprimido exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al descomprimir: {ex.Message}");
            }
        }
    }
}