using System;
using System.IO;
using System.Reflection;

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
                Console.WriteLine("1. Comprimir archivo");
                Console.WriteLine("2. Descomprimir archivo");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");

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
            Console.Write("Ingrese el nombre del archivo (con extensión) a comprimir: ");
            string archivo = Console.ReadLine();

            if (!File.Exists(archivo))
            {
                Console.WriteLine("El archivo no existe.");
                return;
            }

            string ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string nombreArchivo = Path.GetFileName(archivo);
            string archivoBin = Path.Combine(ruta, Path.ChangeExtension(nombreArchivo, ".bin"));
            Console.WriteLine($"El archivo comprimido se guardará en: {archivoBin}");

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
            Console.Write("Ingrese el nombre del archivo a descomprimir (.bin): ");
            string fileName = Console.ReadLine();

            string programDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string archivo = Path.Combine(programDirectory, fileName);

            if (!File.Exists(archivo))
            {
                Console.WriteLine($"El archivo '{archivo}' no existe en el directorio del programa.");
                return;
            }

            string archivoDescomprimido = Path.Combine(programDirectory, Path.ChangeExtension(Path.GetFileNameWithoutExtension(fileName), ".txt"));
            Console.WriteLine($"El archivo descomprimido se guardará en: {archivoDescomprimido}");

            try
            {
                var (textoComprimido, HuffmanTree) = _manager.CargarArchivoComprimido(archivo);
                string textoDescomprimido = _manager.DescomprimirArchivo(textoComprimido, HuffmanTree);
                File.WriteAllText(archivoDescomprimido, textoDescomprimido);
                Console.WriteLine("Archivo descomprimido exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al descomprimir: {ex.Message}");
            }
        }
    }
}