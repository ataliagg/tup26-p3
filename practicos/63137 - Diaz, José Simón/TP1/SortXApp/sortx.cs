try
{
    AppConfig appConfig = ParseArgs(args);

    string rawInputText = ReadInput(appConfig.InputFile);

    var parsedData = ParseDelimited(rawInputText, appConfig.Delimiter, appConfig.NoHeader);

    var sortedRows = SortRows(parsedData, appConfig.SortFields, appConfig.NoHeader);

    string serializedOutput = Serialize((sortedRows, parsedData.Headers), appConfig.Delimiter, appConfig.NoHeader);

    WriteOutput(serializedOutput, appConfig.OutputFile);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Error: {exception.Message}");
    Environment.Exit(1);
}

AppConfig ParseArgs(string[] args) => throw new NotImplementedException();
string ReadInput(string? inputFilePath) => throw new NotImplementedException();

(List<Dictionary<string, string>> Rows, List<string> Headers) ParseDelimited(
    string rawText, string delimiter, bool noHeader) => throw new NotImplementedException();

List<Dictionary<string, string>> SortRows(
    (List<Dictionary<string, string>> Rows, List<string> Headers) parsedData,
    List<SortField> sortFields,
    bool noHeader) => throw new NotImplementedException();

string Serialize(
    (List<Dictionary<string, string>> Rows, List<string> Headers) sortedData,
    string delimiter,
    bool noHeader) => throw new NotImplementedException();

void WriteOutput(string content, string? outputFilePath) => throw new NotImplementedException();

record SortField(string Name, bool Numeric, bool Descending);
record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);