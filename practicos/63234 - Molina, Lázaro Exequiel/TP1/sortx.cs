using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Linq;

record SortField(string Name, bool Numeric);
record AppConfig(string? InF, string? OutF, string Delim, bool NoHeader, List<SortField> Fields);

class Program {
    static void Main(string[] args) {
        var cfg = ParseArgs(args);
        var txt = cfg.InF == null ? Console.In.ReadToEnd() : File.ReadAllText(cfg.InF);
        var lines = ParseLines(txt, cfg.Delim, cfg.NoHeader, out var hdr);
        SortRows(lines, hdr, cfg.Fields);
        OutputRows(hdr, lines, cfg);
    }

    static AppConfig ParseArgs(string[] args) {
        string inFile = null, outFile = null;
        string delim = ",";
        bool noHeader = false;
        var sortFields = new List<SortField>();

        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-i" || args[i] == "--input") {
                inFile = args[++i];
            } else if (args[i] == "-o" || args[i] == "--output") {
                outFile = args[++i];
            } else if (args[i] == "-d" || args[i] == "--delimiter") {
                delim = args[++i].Replace("\\t", "\t").Replace("\\n", "\n");
            } else if (args[i] == "-nh" || args[i] == "--no-header") {
                noHeader = true;
            } else if (args[i] == "-b" || args[i] == "--by") {
                var parts = args[++i].Split(':');
                bool isNumeric = (parts.Length > 1 && parts[1].ToLower() == "num");
                sortFields.Add(new SortField(parts[0], isNumeric));
            } else if (args[i] == "-h" || args[i] == "--help") {
                PrintUsage();
                Environment.Exit(0);
            } else if (!args[i].StartsWith("-")) {
                if (inFile == null) inFile = args[i];
                else if (outFile == null) outFile = args[i];
                else {
                    Console.Error.WriteLine("Too many positional arguments");
                    Environment.Exit(1);
                }
            }
        }

        return new AppConfig(inFile, outFile, delim, noHeader, sortFields);
    }

    static string[][] ParseLines(string text, string delim, bool noHeader, out string[] header) {
        text = text.Replace("\r\n", "\n");
        var allLines = text.Split('\n');
        if (allLines.Length > 0 && allLines[^1] == "") {
            allLines = allLines.Take(allLines.Length - 1).ToArray();
        }

        if (allLines.Length == 0) {
            header = new string[0];
            return new string[0][];
        }

        var rows = new List<string[]>();
        var firstRow = allLines[0].Split(delim);
        int colCount = firstRow.Length;

        if (noHeader) {
            header = Enumerable.Range(0, colCount).Select(x => x.ToString()).ToArray();
            rows.Add(Pad(firstRow, colCount));
        } else {
            header = firstRow;
        }

        for (int i = (noHeader ? 1 : 1); i < allLines.Length; i++) {
            var cols = allLines[i].Split(delim);
            rows.Add(Pad(cols, colCount));
        }

        return rows.ToArray();
    }

    static string[] Pad(string[] arr, int targetLen) {
        if (arr.Length == targetLen) return arr;
        var result = new string[targetLen];
        Array.Copy(arr, result, Math.Min(arr.Length, targetLen));
        for (int i = arr.Length; i < targetLen; i++) {
            result[i] = "";
        }
        return result;
    }

    static void SortRows(string[][] rows, string[] header, List<SortField> sortFields) {
        if (sortFields.Count == 0) return;

        var colIndices = new List<(int idx, bool numeric)>();
        foreach (var field in sortFields) {
            int idx = Array.IndexOf(header, field.Name);
            if (idx < 0) {
                Console.Error.WriteLine($"Column not found: {field.Name}");
                Environment.Exit(1);
            }
            colIndices.Add((idx, field.Numeric));
        }

        Array.Sort(rows, (a, b) => {
            foreach (var (colIdx, isNum) in colIndices) {
                var valA = a[colIdx] ?? "";
                var valB = b[colIdx] ?? "";

                int cmp;
                if (isNum) {
                    var numA = double.TryParse(valA, NumberStyles.Any, CultureInfo.InvariantCulture, out double na);
                    var numB = double.TryParse(valB, NumberStyles.Any, CultureInfo.InvariantCulture, out double nb);
                    if (numA && numB) {
                        cmp = na.CompareTo(nb);
                    } else {
                        cmp = string.Compare(valA, valB);
                    }
                } else {
                    cmp = string.Compare(valA, valB);
                }

                if (cmp != 0) return cmp;
            }
            return 0;
        });
    }

    static void OutputRows(string[] header, string[][] rows, AppConfig cfg) {
        var sb = new StringBuilder();
        
        if (!cfg.NoHeader) {
            sb.AppendLine(string.Join(cfg.Delim, header));
        }

        foreach (var row in rows) {
            sb.AppendLine(string.Join(cfg.Delim, row));
        }

        string output = sb.ToString();
        if (output.EndsWith("\n")) {
            output = output.Substring(0, output.Length - 1);
        }

        if (cfg.OutF == null) {
            Console.Write(output);
        } else {
            File.WriteAllText(cfg.OutF, output);
        }
    }

    static void PrintUsage() {
        Console.WriteLine("Uso: sortx [in [out]] [-b campo[:num]] [-d delim] [-nh] [-h]");
    }
}
