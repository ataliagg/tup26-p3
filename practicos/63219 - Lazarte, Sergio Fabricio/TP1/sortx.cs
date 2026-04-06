try
{
    var config = ParseArgs(args);
    var input = ReadInput(config);
    var rows = ParseDelimited(input, config);
    var sorted = SortRows(rows, config);
    var output = Serialize(sorted, config);
    WriteOutput(output, config);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
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

AppConfig ParseArgs(string[] args)
{
    return new AppConfig(null, null, ",", false, new List<SortField>());
}

string ReadInput(AppConfig config)
{
    return "";
}

List<Dictionary<string, string>> ParseDelimited(string input, AppConfig config)
{
    return new List<Dictionary<string, string>>();
}

List<Dictionary<string, string>> SortRows(List<Dictionary<string, string>> rows, AppConfig config)
{
    return rows;
}

string Serialize(List<Dictionary<string, string>> rows, AppConfig config)
{
    return "";
}

void WriteOutput(string output, AppConfig config)
{
    Console.WriteLine(output);
}