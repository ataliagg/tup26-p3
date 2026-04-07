
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

using System.Net;
using System.Security.AccessControl;
using System.Xml;
using Microsoft.VisualBasic.FileIO;

Console.WriteLine($"sortx {string.Join(" ", args)}");
AppConfig parseargs(string[] args)
{
    string? inputFile = null; 
    string? outputFile = null; 
    string delimiter = ","; 
    bool help = false; 
    bool noHeader = false; 
    List<SortField> sortFields = new List<SortField>(); 
    int positional = 0; 
    sortFields parseSoftfield(string spec)
    {
        var parts = spec.split(','); 
        string name = parts[0];
        bool numeric = parts.length > 1 && parts[1].equals("num", StringComparison.OrdinalIgnoreCase); 
        bool descending = parts.length > 2 && parts[2].equals("desc", StringComparison.OrdinalIgnoreCase); 
        return new SortField(name, numeric, descending); 
    } 

    for (int i = 0; i < args.Length; i++)
    {
        string arg = args[i]; 

        if (arg == "-h" || arg == "--help") ;  
        showhelp() = true; 
        continue;
        if (args == --noheader || args == -nh) ; 
        {
            noheader = true; 
            continue;
        }
        if (arg == -by || arg == -b) ;
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"La opcion '(arg)' requiere un valor.");
            SortField.Add(ParseSortField(args[++i])); 
            continue;
        }
        if (arg == "--input" || arg == "-i")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"La opcion '(arg)' requiere un valor.");
            inputFile = args[++i]; 
            continue;
        }
        if (arg == "--output" || arg == "-o")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"La opcion '(arg)' requiere un valor.");
            outputFile = args[++i]; 
            continue;
        }
        if (arg == "--delimiter" || arg == "-d")
        {
            if (i + 1 >= args.Length)
                throw new ArgumentException($"La opcion '(arg)' requiere un valor.");
            string raw = args[++i];
            delimiter = raw == @"\t" ? "\t" : raw; 
            continue;
        }
        if (!arg.StartsWith('-'))
        {
            if (positional == 0) { inputFile = arg; positional++; }
            else if (positional == 1) { outputFile = arg; positional++; }
            else throw new ArgumentException($"Argumento posicional inesperado: '{arg}'.");
            continue;
        }
        throw new ArgumentException($"Opción desconocida: '{arg}'.");
    }
    return new AppConfig(inputFile, outputFile, delimiter, noHeader, showHelp, sortFields);
}








record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);