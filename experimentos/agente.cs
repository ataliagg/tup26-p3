// NanoProg.cs
// C# 14 file-based app
#:package DiffPlex@1.9.0
#:package Microsoft.Agents.AI@1.1.0
#:package Microsoft.Agents.AI.OpenAI@1.1.0
#:package Microsoft.Extensions.AI@10.5.0
#:package OpenAI@2.10.0

using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

var workspace = new DirectoryInfo(AppContext.BaseDirectory);
var promptFile = Path.Combine(workspace.FullName, "nanop_prompt.txt");

var baseRules = $$"""
    Reglas operativas:
    - Trabajás en {{workspace.FullName}}.
    - Para leer o explorar, usá exec_shell (ls, cat, grep, find) o read_file.
    - Para crear, modificar o borrar archivos, usá apply_patch con un diff unificado.
    - Nunca edites archivos con comandos de shell (heredocs, sed, echo >).
    - Antes de modificar un archivo existente, leelo con read_file.
    - Si apply_patch falla, reportá el error y no reintentes.
    - No inventes resultados. Respondé corto.
    """;

var instructions = File.Exists(promptFile)
    ? $"{File.ReadAllText(promptFile).Replace("{workspace}", workspace.FullName).Trim()}\n\n{baseRules}"
    : baseRules;

var tools = new FileSystemTools(workspace);

AIAgent agent = new OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_API_KEY"))
    .GetChatClient("gpt-5.4")
    .CreateAIAgent(
        instructions: instructions,
        name: "NanoProg",
        tools: [
            AIFunctionFactory.Create(tools.ExecShell),
            AIFunctionFactory.Create(tools.ReadFile),
            AIFunctionFactory.Create(tools.ApplyPatch),
        ]);

AgentThread thread = agent.GetNewThread();

while (true)
{
    Console.Write("Tú> ");
    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(input)) continue;
    if (input is "salir" or "exit" or "quit") break;

    var response = await agent.RunAsync(input, thread);
    Console.WriteLine($"\nAgente> {response.Text}\n");
}


sealed class FileSystemTools(DirectoryInfo workspace)
{
    [Description("Ejecuta uno o varios comandos de shell en el workspace y devuelve stdout, stderr y exit code.")]
    public string ExecShell(
        [Description("Comandos de shell a ejecutar en secuencia.")] string[] commands)
    {
        var sb = new StringBuilder();
        foreach (var cmd in commands)
        {
            var psi = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/sh",
                Arguments = OperatingSystem.IsWindows() ? $"/c {cmd}" : $"-c \"{cmd.Replace("\"", "\\\"")}\"",
                WorkingDirectory = workspace.FullName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            using var p = Process.Start(psi)!;
            var stdout = p.StandardOutput.ReadToEnd();
            var stderr = p.StandardError.ReadToEnd();
            p.WaitForExit();
            sb.AppendLine($"$ {cmd}")
              .AppendLine($"exit_code: {p.ExitCode}")
              .AppendLine("--- STDOUT ---").AppendLine(stdout)
              .AppendLine("--- STDERR ---").AppendLine(stderr);
        }
        return sb.ToString();
    }

    [Description("Lee el contenido completo de un archivo del workspace.")]
    public string ReadFile([Description("Ruta relativa al workspace.")] string path)
        => File.ReadAllText(Resolve(path));

    [Description(
        "Aplica una operación sobre un archivo: create, update o delete. " +
        "Para create/update, 'diff' es un diff unificado relativo al contenido actual (vacío si es create).")]
    public string ApplyPatch(
        [Description("Operación: 'create', 'update' o 'delete'.")] string type,
        [Description("Ruta relativa al workspace.")] string path,
        [Description("Diff unificado. Omitir para 'delete'.")] string? diff = null)
    {
        var target = Resolve(path);
        switch (type)
        {
            case "create":
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.WriteAllText(target, ApplyDiff("", diff ?? ""));
                return $"Creado {path}";
            case "update":
                File.WriteAllText(target, ApplyDiff(File.ReadAllText(target), diff ?? ""));
                return $"Actualizado {path}";
            case "delete":
                File.Delete(target);
                return $"Borrado {path}";
            default:
                throw new ArgumentException($"Tipo inválido: {type}");
        }
    }

    string Resolve(string relative)
    {
        var full = Path.GetFullPath(Path.Combine(workspace.FullName, relative));
        if (!full.StartsWith(workspace.FullName, StringComparison.Ordinal))
            throw new UnauthorizedAccessException($"Ruta fuera del workspace: {relative}");
        return full;
    }

    static string ApplyDiff(string original, string diff)
    {
        if (string.IsNullOrEmpty(diff)) return original;

        var result = new List<string>(original.Split('\n'));
        var diffLines = diff.Split('\n');
        var cursor = 0;

        foreach (var line in diffLines)
        {
            if (line.StartsWith("@@") || line.StartsWith("---") || line.StartsWith("+++")) continue;
            if (line.Length == 0) continue;

            switch (line[0])
            {
                case ' ':
                    while (cursor < result.Count && result[cursor] != line[1..]) cursor++;
                    cursor++;
                    break;
                case '-':
                    var idx = result.IndexOf(line[1..], cursor);
                    if (idx < 0) throw new InvalidOperationException($"Contexto no encontrado: {line}");
                    result.RemoveAt(idx);
                    cursor = idx;
                    break;
                case '+':
                    result.Insert(cursor, line[1..]);
                    cursor++;
                    break;
            }
        }
        return string.Join('\n', result);
    }
}