
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

using System.Net.Http.Headers;
using Microsoft.VisualBasic.FileIO;

Console.WriteLine($"sortx {string.Join(" ", args)}");


// este appconfig guarda los datos temporalmente, a diferencia del record que los toma al final
AppConfig parseargs(string[] args)
{
    string? inputFile = null; 
    string? outputFile = null; 
    string deLimiter = ","; 
    bool noHeader = false; 
    bool showHelp = false;
    List<SortField> sortFields = new List<SortField>(); 
    int positional = 0; 

    SortField ParseSortField(string spec)
    {
        var parts = spec.Split(':');
        string name = parts[0];
        bool numeric = parts.Length > 1 && parts[1].Equals("num", StringComparison.OrdinalIgnoreCase);
        bool descending = parts.Length > 2 && parts[2].Equals("desc", StringComparison.OrdinalIgnoreCase);
        return new SortField(name, numeric, descending);}
    

    for (int i = 0; i < args.Length; i++)
    {
        string arg = args[i];

        if (arg == "--help" || arg == "-h")
        
            showHelp = true;
            continue;
        

        if (arg == "--no-header" || arg == "-nh")
        {
            noHeader = true;
            continue;
        

        if (arg == "--by" || arg == "-b")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"necesita '{arg}' un valor.");

            sortFields.Add(ParseSortField(args[++i]));
            continue;
        

        if (arg == "--input" || arg == "-i")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"necesita '{arg}' un valor.");

            inputFile = args[++i];
            continue;
        }

        if (arg == "--output" || arg == "-o")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"necesita '{arg}' un valor.");

            outputFile = args[++i];
            continue;
        }

        if (arg == "--deLimiter" || arg == "-d")
        
            if (i + 1 >= args.Length)
                throw new ArgumentException($"necesita '{arg}' un valor.");

            string raw = args[++i];
            deLimiter = raw == @"\t" ? "\t" : raw;
            continue;
        }

        if (!arg.StartsWith("-"))
        
            if (positional == 0) { inputFile = arg; positional++; }
            else if (positional == 1) { outputFile = arg; positional++; }
            else throw new ArgumentException($"arg posicional inexistente: '{arg}'.");
            continue;
        }

        throw new ArgumentException($"opción desconocida: '{arg}'.");
    }

    return new AppConfig(inputFile, outputFile, delimiter, noHeader, showHelp, sortFields);

}
string readinput(AppConfig cfg)
{
    //  se verifica que el archivo cfg no sea null (los datos parseadso)
    if(cfg.InputFile == null) 
      return File.ReadAllText(cfg.InputFile); 
      return Console.In.ReadToEnd(); 
} 
// lista de fila y encabezado en base al texto de archivo cfg
(List<Dictionary<string,string>> rows,string[]? Header) parsedelimited (string text, AppConfigconfig cfg)
{
    // el templines va a tener todsas las lineas incluyendo las vacias tambien, despues con el split separa en lineas y por ultimo se eliminan las lineas vacias.
  var tempLines = text
    .Replace("\r\n", "\n")
    .Replace("\r", "\n")
    .Split('\n');
string[] lines = Array.FindAll(tempLines, l => l.Length > 0);
}

    if (lines.length == 0)
     return (new List<Dictionary<string, string>>(), null);



    string[] header;
    int dataStart;

    if (!cfg.NoHeader)
    {
        header = lines[0].Split(cfg.Delimiter);
        dataStart = 1;
    }
    else
    {
       headers = null;
       dataStart = 0;
    }
        var rows = new List<Dictionary<string, string>>(); //guardo los datos de  listas en esta variable rows
    for (int lineIdx =dataStart; lineIdx < lines.Length; lineIdx++) // recorre las lineas del archivo en base al data start y si es q hay encabezado o no

    var values = lines[lineIdx].Split(cfg.Delimiter); // se guardan los datos spliteados en values

    var row = new Dictionary<string, string>(); // se crea una carpeta vacia para guardar los datos de cada fila.
  
    if (!cfg.noheader && header is not null)
    {
        for (int col = 0; col < header.Length; col++) // bucle q recorre las columnas de las columnas 0 al 3
        row[header[col]] = col < values.Length ? values[col] : string.Empty; // se guarda en  la var row el valor de cada columna
    }
    else
    {
        for (int col = 0; col < values.Length; col++)
        row[col.ToString()] = values[col];
    }
    if (cfg.sortFields.Count > 0 && row.Count > 0) //toma en cuenta la cantidad de elementos de sort fields y la cantidad de elementos de row 
{
    var firstrow = rows[0]; // guarda el valor de la primera fila de rows en la variable firstrow
    foreach (var sf in cfg.sortFields) //bucle en donde la variable sf es temporal y toma los datos de cfg sort fields
    {
        if (!firstrow.ContainsKey(sf.Name)) // si no existe la condicion de que la primera fila contenga la clave del campo de ordenamiento
        {
            string available = "";
            foreach (var key in firstrow.Keys) //bucle mostrando las columnas de row y se guardan en key temporalmente 
            {
                if (available.Length > 0) 
                available += ", ";
                available += key; 
                // se verifica si la var available tiene algun valor, si es que tiene se le agrega una , y despues el nombre de la columna que esta en key
            }
         throw newargumentException($"campo de ordenamiento desconocido: '{sf.Name}'. disponible: {available}"); // error si es que la primera fila no tiene clave sf
        }
    }
    return (rows, header); // devuelve las filas y el encabezado









 //commit 1
record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    bool ShowHelp,
    List<SortField> SortFields
);}


