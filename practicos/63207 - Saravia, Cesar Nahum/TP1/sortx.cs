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

