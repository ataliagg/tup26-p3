
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

record SortField(string Name, bool Numeric, bool Descending);
record AppConfig(string? InputFile, string? OutputFile, string Delimiter, bool NoHeader, List<SortField> SortFields);

class Program
{
    static int Main(string[] args)
    {
        try
        {
            var config = ParseArgs(args);
            var texto = ReadInput(config);
            var (header, filas) = ParseDelimited(config, texto);
            var ordenadas = SortRows(config, header, filas);
            var salida = Serialize(config, header, ordenadas);
            WriteOutput(config, salida);
            return 0;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
            return 1;
        }
    }

    static AppConfig? ParseArgs(string[] args) { return null; }
    static string ReadInput(AppConfig config) { return ""; }
    static (List<string>, List<List<string>>) ParseDelimited(AppConfig config, string texto) { return (new List<string>(), new List<List<string>>()); }
    static List<List<string>> SortRows(AppConfig config, List<string> header, List<List<string>> filas) { return filas; }
    static string Serialize(AppConfig config, List<string> header, List<List<string>> filas) { return ""; }
    static void WriteOutput(AppConfig config, string texto) { }
}