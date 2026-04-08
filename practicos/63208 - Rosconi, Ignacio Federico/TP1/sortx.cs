
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

Console.WriteLine($"sortx {string.Join(" ", args)}");

using System.Text;

record SortField(string Name, bool IsNumeric, bool IsDescending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields,
    bool HelpRequested
);

try
{
    var config = ParseArgs(args);

    if (config.HelpRequested)
    {
        PrintHelp();
        return;
    }

    var raw = ReadInput(config);
    var parsed = ParseDelimited(raw, config);
    var sorted = SortRows(parsed.Headers, parsed.Rows, config);
    var text = Serialize(parsed.Headers, sorted, config);
    WriteOutput(text, config);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}

// ===== FUNCIONES (VACÍAS POR AHORA) =====

AppConfig ParseArgs(string[] args)
{
    throw new NotImplementedException();
}

string ReadInput(AppConfig config)
{
    throw new NotImplementedException();
}

(List<string> Headers, List<List<string>> Rows)
ParseDelimited(string text, AppConfig config)
{
    throw new NotImplementedException();
}

List<List<string>> SortRows(
    List<string> headers,
    List<List<string>> rows,
    AppConfig config)
{
    throw new NotImplementedException();
}

string Serialize(
    List<string> headers,
    List<List<string>> rows,
    AppConfig config)
{
    throw new NotImplementedException();
}

void WriteOutput(string text, AppConfig config)
{
    throw new NotImplementedException();
}

void PrintHelp()
{
    throw new NotImplementedException();
}