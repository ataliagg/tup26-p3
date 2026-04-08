using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

try
{
    Console.WriteLine("sortx iniciado");
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error: " + ex.Message);
    Environment.Exit(1);
}

record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);