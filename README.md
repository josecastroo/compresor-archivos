# compresor-archivos

## Instrucciones de Uso

### Menú Principal
Al ejecutar el programa, verás el siguiente menú:
    Menú
1. Comprimir archivo
2. Descomprimir archivo
3. Salir

### Explicación de Cada Sección del Menú
**Opción 1: Comprimir archivo**
Esta opción toma un archivo de texto (.txt) ubicado en la carpeta del proyecto CompresorArchivos\CompresorArchivos\, analiza la frecuencia de aparición de cada carácter en el archivo, construye un árbol de Huffman basado en estas frecuencias, genera códigos binarios óptimos para cada carácter (los caracteres más frecuentes obtienen códigos más cortos), comprime el contenido completo del archivo reemplazando cada carácter por su código binario correspondiente, empaqueta estos bits en bytes, y finalmente guarda el resultado en formato binario (.bin) junto con los metadatos necesarios para la descompresión (tabla de frecuencias). Durante el proceso, el programa muestra en pantalla el contenido original del archivo, una tabla detallada con las frecuencias y porcentajes de cada símbolo, y estadísticas completas de compresión incluyendo el tamaño original, tamaño comprimido, proporción de compresión y tiempo de ejecución en milisegundos.

**Opción 2: Descomprimir archivo**
Esta opción toma un archivo comprimido (.bin) que debe estar en la carpeta bin\Debug\net8.0\, lee los metadatos del archivo (número de símbolos y tabla de frecuencias), reconstruye exactamente el mismo árbol de Huffman que se usó durante la compresión utilizando las frecuencias guardadas, lee la secuencia de bits comprimidos byte por byte y los desempaqueta, navega el árbol de Huffman bit por bit (siguiendo el camino izquierdo para '1' y derecho para '0'), identifica cada carácter original cuando llega a una hoja del árbol, reconstruye el texto completo carácter por carácter hasta procesar todos los bits, y finalmente guarda el resultado como un archivo de texto (.txt) que es idéntico al archivo original antes de la compresión. El programa también muestra en pantalla el contenido del archivo descomprimido para verificar que la restauración fue exitosa.

**Opción 3: Salir**
Esta opción cierra el programa de forma segura y retorna al sistema operativo.

### Archivos de Ejemplo
El proyecto incluye dos archivos de texto de ejemplo en la carpeta CompresorArchivos\CompresorArchivos\:
archivo.txt - Es un archivo bastante normal con texto simple ("Hola mundo!"). Este archivo es ideal para realizar una primera prueba rápida del programa y entender el funcionamiento básico del sistema de compresión y descompresión. Debido a su pequeño tamaño y baja repetición de caracteres, no mostrará una compresión significativa, pero permite verificar que todo funciona correctamente.
altaComprension.txt - Es un archivo especialmente diseñado con altísima repetición de patrones. Contiene secuencias largas de los mismos caracteres repetidos (como "A A A A A A..."), frases completas repetidas múltiples veces (como "La casa es grande" repetida varias veces), palabras repetidas en secuencia (como "hola hola hola hola..."), y patrones específicos repetidos (como "ababababa..." y "xyzxyzxyz..."). Este archivo está optimizado para demostrar la verdadera efectividad del algoritmo de Huffman, ya que los caracteres y patrones que se repiten con alta frecuencia reciben códigos binarios muy cortos, resultando en tasas de compresión superiores al 50%, permitiendo probar mejor el algoritmo y ver resultados realmente impresionantes de reducción de tamaño.

### Ubicación de Archivos Comprimidos y Descomprimidos
Importante: Todos los archivos comprimidos (.bin) generados por la opción "Comprimir archivo" y todos los archivos descomprimidos (.txt) generados por la opción "Descomprimir archivo" siempre van a estar guardados en la misma carpeta bin\Debug\net8.0\, independientemente de dónde esté ubicado el archivo original que se está comprimiendo. Esta carpeta se encuentra dentro de la estructura del proyecto después de compilar y ejecutar el programa. Cuando comprimes un archivo, el programa automáticamente toma el archivo de la carpeta del proyecto, lo procesa, y guarda el resultado .bin en bin\Debug\net8.0\. Cuando descomprimes, el programa busca el archivo .bin en bin\Debug\net8.0\ y guarda el archivo restaurado .txt en la misma ubicación bin\Debug\net8.0\.

## Decisiones de Diseño

**1. Separación de Responsabilidades**
*Decisión:* Dividir el sistema en clases especializadas (Tokenizer, Compresor, Descompresor, HuffmanTree, CompresorManager, View).
*Justificación:* Cada clase tiene una única responsabilidad claramente definida. Tokenizer solo analiza frecuencias, Compresor solo codifica, Descompresor solo decodifica, HuffmanTree maneja la estructura del árbol, CompresorManager coordina las operaciones, y View maneja la interfaz de usuario. Esto hace el código más fácil de mantener, probar y modificar sin afectar otros componentes.

**2. Formato de Archivo Binario con Metadatos Embebidos**
*Decisión:* Guardar la tabla de frecuencias completa dentro del archivo .bin junto con los datos comprimidos.
*Justificación:* Aunque esto aumenta el tamaño del archivo comprimido (especialmente en archivos pequeños), permite que el archivo sea autocontenido - puede ser descomprimido sin necesidad de archivos externos o información adicional. El descompresor reconstruye el árbol de Huffman idéntico usando las frecuencias guardadas, garantizando una descompresión exacta. Es un trade-off entre portabilidad y eficiencia de espacio.

**3. Construcción del Árbol mediante Ordenamiento Iterativo**
*Decisión:* Usar OrderBy() de LINQ en cada iteración al construir el árbol de Huffman en lugar de implementar una Priority Queue.
*Justificación:* Para archivos de texto típicos con un número limitado de caracteres únicos (normalmente < 256), la diferencia de rendimiento entre O(k² log k) y O(k log k) es negligible en la práctica. Esta implementación prioriza la claridad y simplicidad del código sobre la máxima eficiencia teórica, haciéndolo más fácil de entender con propósitos educativos sin necesidad de implementar estructuras de datos adicionales.

**4. Navegación del Árbol para Descompresión**
*Decisión:* Descomprimir navegando el árbol bit por bit (izquierda para '1', derecha para '0') en lugar de crear un diccionario inverso.
*Justificación:* Esta aproximación es la implementación estándar del algoritmo de Huffman y no requiere memoria adicional para almacenar un diccionario inverso. Aunque requiere recorrer el árbol para cada carácter, la complejidad sigue siendo O(n) donde n es el número de bits, y el árbol de Huffman tiene profundidad logarítmica promedio, haciendo las búsquedas muy rápidas. Es una solución elegante que aprovecha directamente la estructura del árbol.