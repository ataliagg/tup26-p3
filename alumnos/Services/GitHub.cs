namespace Tup26.AlumnosApp;

class GitHub {
    readonly string owner;
    readonly string repo;

    public GitHub(string owner = "AlejandroDiBattista", string repo = "tup26-p3") {
        this.owner = owner;
        this.repo = repo;
    }

    public bool AgregarColaborador(string usuario) {
        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", "--method", "PUT", $"repos/{owner}/{repo}/collaborators/{usuario}", "-f", "permission=push" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al agregar colaborador '{usuario}': {detalle}");
            return false;
        }

        return true;
    }

    public List<string> ListarColaboradores() {
        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", $"repos/{owner}/{repo}/collaborators", "--jq", ".[] | select(.permissions.push == true) | .login" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al listar colaboradores: {detalle}");
            return new();
        }

        return LeerLineas(salida);
    }

    public List<string> ListarInvitacionesPendientes() {
        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", $"repos/{owner}/{repo}/invitations", "--paginate", "--jq", ".[].invitee.login" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al listar invitaciones pendientes: {detalle}");
            return new();
        }

        return LeerLineas(salida);
    }

    public List<(int Numero, string Titulo, bool? EsMergeable, bool EstaAbierto)> ListarPRs(bool soloAbiertos = true) {
        string estado = soloAbiertos ? "open" : "all";

        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", $"repos/{owner}/{repo}/pulls?state={estado}", "--paginate", "--jq", ".[] | \"\\(.number)\\t\\(.title)\\t\\(.state)\"" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al listar PRs: {detalle}");
            return new();
        }

        List<(int Numero, string Titulo, bool? EsMergeable, bool EstaAbierto)> prs = new();

        foreach (string linea in LeerLineas(salida, pasarAMinusculas: false)) {
            string[] partes = linea.Split('\t', 3);

            if (partes.Length != 3) {
                continue;
            }

            if (!int.TryParse(partes[0], out int numero)) {
                continue;
            }

            bool estaAbierto = string.Equals(partes[2], "open", StringComparison.OrdinalIgnoreCase);
            bool? esMergeable = estaAbierto ? ObtenerMergeablePR(numero) : null;

            prs.Add((numero, partes[1], esMergeable, estaAbierto));
        }

        return prs;
    }

    public bool CambiarTituloPR(int numeroPR, string nuevoTitulo) {
        if (string.IsNullOrWhiteSpace(nuevoTitulo)) {
            Console.WriteLine("Error al cambiar el título del PR: el nuevo título no puede estar vacío.");
            return false;
        }

        string titulo = nuevoTitulo.Trim();

        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", "--method", "PATCH", $"repos/{owner}/{repo}/pulls/{numeroPR}", "-f", $"title={titulo}" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al cambiar el título del PR #{numeroPR}: {detalle}");
            return false;
        }

        return true;
    }

    public List<(string Titulo, DateTimeOffset FechaHora)> ListarCommitsPR(int numeroPR) {
        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", $"repos/{owner}/{repo}/pulls/{numeroPR}/commits", "--paginate", "--jq", ".[] | \"\\(.commit.message | split(\"\\n\")[0])\\t\\(.commit.author.date)\"" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al listar commits del PR #{numeroPR}: {detalle}");
            return new();
        }

        List<(string Titulo, DateTimeOffset FechaHora)> commits = new();

        foreach (string linea in LeerLineas(salida, pasarAMinusculas: false)) {
            string[] partes = linea.Split('\t', 2);

            if (partes.Length != 2) {
                continue;
            }

            if (!DateTimeOffset.TryParse(partes[1], out DateTimeOffset fechaHora)) {
                continue;
            }

            commits.Add((partes[0], fechaHora));
        }

        return commits;
    }

    bool? ObtenerMergeablePR(int numeroPR) {
        (string salida, string error, int codigoSalida) = EjecutarGh(new[]
        { "api", $"repos/{owner}/{repo}/pulls/{numeroPR}", "--jq", ".mergeable" });

        if (codigoSalida != 0) {
            string detalle = string.IsNullOrWhiteSpace(error) ? salida : error;
            Console.WriteLine($"Error al consultar si el PR #{numeroPR} es mergeable: {detalle}");
            return null;
        }

        string valor = salida.Trim();

        if (string.Equals(valor, "true", StringComparison.OrdinalIgnoreCase)) {
            return true;
        }

        if (string.Equals(valor, "false", StringComparison.OrdinalIgnoreCase)) {
            return false;
        }

        return null;
    }

    (string Salida, string Error, int CodigoSalida) EjecutarGh(IEnumerable<string> argumentos) {
        ProcessStartInfo startInfo = new ProcessStartInfo {
            FileName = "gh",
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        foreach (string argumento in argumentos) {
            startInfo.ArgumentList.Add(argumento);
        }

        // Console.WriteLine($"Ejecutando gh {string.Join(' ', argumentos)}...");

        using Process proceso = Process.Start(startInfo)
            ?? throw new InvalidOperationException("No se pudo iniciar gh.");

        string salida = proceso.StandardOutput.ReadToEnd().Trim();
        string error = proceso.StandardError.ReadToEnd().Trim();

        proceso.WaitForExit();

        return (salida, error, proceso.ExitCode);
    }

    

    static List<string> LeerLineas(string texto, bool pasarAMinusculas = true) {
        return texto.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(linea => linea.Trim())
                    .Select(linea => pasarAMinusculas ? linea.ToLower() : linea)
                    .Where(linea => !string.IsNullOrWhiteSpace(linea))
                    .ToList();
    }

    
}
