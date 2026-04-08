using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

try
{
    var config = ParseArgs(args);
    Console.WriteLine("Argumentos procesados");
}
catch (Exception ex)
{
    Console.Error.WriteLine("Error: " + ex.Message);
    Environment.Exit(1);
}

AppConfig ParseArgs(string[] args)
{
    string? input = null;
    string? output = null;
    string delimiter = ",";
    bool noHeader = false;
    var camposOrden = new List<SortField>();
    var posicionales = new List<string>();

    for (int i = 0; i < args.Length; i++)
    {
        var arg = args[i];

        switch (arg)
        {
            case "-h":
            case "--help":
                ShowHelp();
                Environment.Exit(0);
                break;
        }
    }

    return new AppConfig(input, output, delimiter, noHeader, camposOrden);
}

void ShowHelp()
{
    Console.WriteLine("Uso: sortx");
}

record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);
