using System.Text;

namespace VisiCalc;

internal static class CsvFormat {
    public static char DetectDelimiter(string text) {
        if (string.IsNullOrEmpty(text)) {
            return ',';
        }

        string firstLine = text
            .Split(['\r', '\n'], 2, StringSplitOptions.None)
            .FirstOrDefault() ?? string.Empty;

        char[] candidates = [',', ';', '\t'];
        return candidates
            .Select(candidate => new { Candidate = candidate, Count = firstLine.Count(c => c == candidate) })
            .OrderByDescending(item => item.Count)
            .ThenBy(item => item.Candidate == ',' ? 0 : 1)
            .First().Candidate;
    }

    public static List<List<string>> Parse(string text, char delimiter) {
        List<List<string>> rows = [];
        List<string> row = [];
        StringBuilder field = new();
        bool inQuotes = false;

        void FinishField() {
            row.Add(field.ToString());
            field.Clear();
        }

        void FinishRow() {
            FinishField();
            rows.Add(row);
            row = [];
        }

        for (int i = 0; i < text.Length; i++) {
            char c = text[i];

            if (inQuotes) {
                if (c == '"') {
                    if (i + 1 < text.Length && text[i + 1] == '"') {
                        field.Append('"');
                        i++;
                    } else {
                        inQuotes = false;
                    }
                } else {
                    field.Append(c);
                }

                continue;
            }

            if (c == '"') {
                inQuotes = true;
                continue;
            }

            if (c == delimiter) {
                FinishField();
                continue;
            }

            if (c == '\r') {
                if (i + 1 < text.Length && text[i + 1] == '\n') {
                    i++;
                }

                FinishRow();
                continue;
            }

            if (c == '\n') {
                FinishRow();
                continue;
            }

            field.Append(c);
        }

        if (field.Length > 0 || row.Count > 0 || rows.Count == 0) {
            FinishRow();
        }

        if (rows.Count > 1 && rows[^1].Count == 1 && rows[^1][0].Length == 0 && text.EndsWith('\n')) {
            rows.RemoveAt(rows.Count - 1);
        }

        return rows;
    }

    public static string Write(IReadOnlyList<IReadOnlyList<string>> rows, char delimiter) {
        StringBuilder builder = new();

        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++) {
            IReadOnlyList<string> row = rows[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Count; columnIndex++) {
                if (columnIndex > 0) {
                    builder.Append(delimiter);
                }

                builder.Append(Escape(row[columnIndex], delimiter));
            }

            if (rowIndex < rows.Count - 1) {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private static string Escape(string value, char delimiter) {
        bool needsQuotes = value.Contains(delimiter) || value.Contains('"') || value.Contains('\r') || value.Contains('\n');
        if (!needsQuotes) {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}
