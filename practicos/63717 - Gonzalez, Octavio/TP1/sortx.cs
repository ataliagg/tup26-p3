using System;
using static System.Console; 


/*
sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
      [-i|--input input] [-o|--output output]
      [-d|--delimiter delimitador]
      [-nh|--no-header] [-h|--help]
| Opción larga    | Corta  | Descripción |
|-----------------|--------|-------------|
| `--by`          | `-b`   | Campo por el que ordenar. Se puede repetir para ordenamiento múltiple. |
| `--input`       | `-i`   | Archivo de entrada. |
| `--output`      | `-o`   | Archivo de salida. |
| `--delimiter`   | `-d`   | Carácter delimitador. Default: `,`. Usar `\t` para tabulación. |
| `--no-header`   | `-nh`  | Indica que el archivo no tiene fila de encabezado. En ese caso los campos se identifican por su índice numérico (0, 1, 2...). |
| `--help`        | `-h`   | Muestra la ayuda y termina. |

Especificación de campo: `campo[:tipo[:orden]]`

Cada valor de `--by` tiene el formato `campo[:tipo[:orden]]`, donde:

- **`campo`** — nombre de la columna (si hay encabezado) o índice numérico desde 0 (si no hay encabezado).
- **`tipo`** — criterio de comparación:
  - `alpha` — comparación alfabética (default).
  - `num` — comparación numérica.
- **`orden`** — dirección:
  - `asc` — ascendente (default).
  - `desc` — descendente.


1. ParseArgs      → leer la configuración desde los argumentos
2. ReadInput      → leer el texto desde el archivo o stdin
3. ParseDelimited → convertir el texto en una lista de filas (lista de diccionarios)
4. SortRows       → ordenar las filas según los criterios configurados
5. Serialize      → convertir las filas ordenadas de vuelta a texto
6. WriteOutput    → escribir en el archivo de salida o stdout

punto de entrada (`try/catch` principal) debe limitarse a invocar estas funciones en orden, sin lógica adicional.

Si el archivo de entrada no se especifica, la herramienta debe leer desde stdin.
Si el archivo de salida no se especifica, la herramienta debe escribir en stdout.

*/
try
{
    AppConfig configuracion= ParseArgs (args); 
    string texto = ReadInput(configuracion); 

}
catch (Exception e)
{
    Error.WriteLine("Error encontrado:"+ e.Message);
    Environment.Exit(1); // avisamos que el programa fallo 
}


AppConfig ParseArgs(string[] args) {

 string? entrada = Console.ReadLine();   
    
}; //Retornara el record AppConfig la funcion es ParseArgs 

static string ReadInput (string? configuracion)
{
    if (configuracion.Entrada!= null)
    {
        return File.ReadAllText(configuracion.Entrada);
    }
    else
    {
        return Console.ReadLine();
    }
}






record SortField (string Nombre, bool Numero, bool Descendente); // campo por el que ordena. 
record AppConfig (string? Entrada, string? Salida, string Delimitador, bool noheader, List<SortField> sortfields);

