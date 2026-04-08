using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

try
{
    var config = ParseArgs(args);
    var texto = ReadInput(config);
    var filas = ParseDelimited(texto, config);
    Console.WriteLine($"Filas leídas: {filas.Count}");
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error: " + ex.Message);
    Environment.Exit(1);
}

AppConfig ParseArgs(string[] args)
{
    return new AppConfig(null, null, ",", false, new List<SortField>());
}

string ReadInput(AppConfig config)
{
    if (Console.IsInputRedirected)
        return Console.In.ReadToEnd();

    return "";
}

List<Dictionary<string, string>> ParseDelimited(string texto, AppConfig config)
{
    return new List<Dictionary<string, string>>();
}

record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);
