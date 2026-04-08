using System.Text;

try
{
    var config = ParseArgs(args);
    var text = ReadInput(config.InputFile);
    var (headers, rows) = ParseDelimited(text, config);
    var sorted = SortRows(rows, headers, config);
    var output = Serialize(headers, sorted, config);
    WriteOutput(config.OutputFile, output);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}

AppConfig ParseArgs(string[] args)
{
    string? input = null;
    string? output = null;
    string delimiter = ",";
    bool noHeader = false;
    var fields = new List<SortField>();
    var positional = new List<string>();

    for (int i = 0; i < args.Length;)
    {
        var arg = args[i];

        switch (arg)
        {
            case "-h":
            case "--help":
                PrintHelp();
                Environment.Exit(0);
                break;

            case "-i":
            case "--input":
                input = Next(args, ref i, arg);
                break;

            case "-o":
            case "--output":
                output = Next(args, ref i, arg);
                break;

            case "-d":
            case "--delimiter":
                delimiter = Next(args, ref i, arg);
                if (delimiter == "\\t") delimiter = "\t";
                break;

            case "-nh":
            case "--no-header":
                noHeader = true;
                i++;
                break;

            case "-b":
            case "--by":
                fields.Add(ParseSortField(Next(args, ref i, arg)));
                break;

            default:
                if (arg.StartsWith("-"))
                    throw new Exception($"Opción desconocida: {arg}");

                positional.Add(arg);
                i++;
                break;
        }
    }

    if (positional.Count >= 1 && input == null) input = positional[0];
    if (positional.Count >= 2 && output == null) output = positional[1];

    if (fields.Count == 0)
        throw new Exception("Debe indicar al menos un campo con -b");

    return new AppConfig(input, output, delimiter, noHeader, fields);
}

void PrintHelp()
{
    Console.WriteLine("Uso: sortx [input [output]] -b campo[:tipo[:orden]]");
}


record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);
