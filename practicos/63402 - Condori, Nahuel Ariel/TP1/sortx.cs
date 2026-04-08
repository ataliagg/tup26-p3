using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

try
{
    var config = ParseArgs(args);
    if (config == null) return 0;

    string inputText = ReadInput(config);
    var (header, rows) = ParseDelimited(inputText, config);
    var sortedRows = SortRows(rows, config);
    string outputText = Serialize(header, sortedRows, config);
    WriteOutput(outputText, config);

    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}