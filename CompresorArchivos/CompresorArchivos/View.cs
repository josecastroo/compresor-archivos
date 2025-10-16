using System;
using System.IO;
using System.Linq;
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
            string arch = Console.ReadLine();

            // donde debe buscar los archivos
            string rutaPrin = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\"));
            string archivo = Path.Combine(rutaPrin, arch);

            if (!File.Exists(archivo))
            {
                Console.WriteLine($"El archivo '{arch}' no fue encontrado en la carpeta principal del proyecto: {rutaPrin}");
                return;
            }

            Console.WriteLine("\nContenido del archivo original");
            string contenido = File.ReadAllText(archivo);
            Console.WriteLine(contenido);
            Console.WriteLine();

            string ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string nombreArchivo = Path.GetFileName(archivo);
            string archivoBin = Path.Combine(ruta, Path.ChangeExtension(nombreArchivo, ".bin"));
            Console.WriteLine($"El archivo comprimido se guardará en: {archivoBin}");

            try
            {
                var (textoComprimido, _, frequencies) = _manager.ComprimirArchivo(archivo);
                _manager.GuardarArchivoComprimido(archivoBin, textoComprimido, frequencies);
                Console.WriteLine("Archivo comprimido exitosamente.");
                MostrarTablaFrecuencias(frequencies);
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

                Console.WriteLine("\nContenido del archivo descomprimido");
                Console.WriteLine(textoDescomprimido);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al descomprimir: {ex.Message}");
            }
        }

        private void MostrarTablaFrecuencias(Dictionary<char, int> frequencies)
        {
            Console.WriteLine("\nCálculo de la frecuencia de cada símbolo:");
            Console.WriteLine("┌─────────┬───────────┬────────────┐");
            Console.WriteLine("│ Símbolo │ Frecuencia│ Porcentaje │");
            Console.WriteLine("├─────────┼───────────┼────────────┤");

            int totalSimbolos = frequencies.Values.Sum();

            foreach (var caracter in frequencies.OrderByDescending(par => par.Value))
            {
                double porcentaje = (double)caracter.Value / totalSimbolos * 100;
                string simbolo = caracter.Key.ToString();
                if (char.IsWhiteSpace(caracter.Key))
                {
                    simbolo = caracter.Key switch
                    {
                        ' ' => "' '",
                        '\n' => "\\n",
                        '\r' => "\\r",
                        '\t' => "\\t",
                        _ => "???"
                    };
                }
                Console.WriteLine($"│ {simbolo,-7} │ {caracter.Value,-9} │ {porcentaje,9:F2}% │");
            }

            Console.WriteLine("└─────────┴───────────┴────────────┘");
        }
    }
}