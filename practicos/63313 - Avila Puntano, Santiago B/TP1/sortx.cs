
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

 //commit 1
record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    bool ShowHelp,
    List<SortField> SortFields
);


