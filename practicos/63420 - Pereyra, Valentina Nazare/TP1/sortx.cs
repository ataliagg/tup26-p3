
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

Console.WriteLine($"sortx {string.Join(" ", args)}");

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

record CampoOrden(string Nombre, bool Descendente);

record Configuracion(string Entrada, string Salida, bool Descendente, List<CampoOrden> Campos);

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando aplicación...");
    }

    static Configuracion ObtenerConfiguracion(string[] args)
    {
        throw new NotImplementedException();
    }

    static List<string> LeerArchivo(string ruta)
    {
        throw new NotImplementedException();
    }

    static List<string> OrdenarLineas(List<string> lineas, Configuracion config)
    {
        throw new NotImplementedException();
    }

    static void GuardarArchivo(string ruta, List<string> lineas)
    {
        throw new NotImplementedException();
    }

    static void MostrarAyuda()
    {
        throw new NotImplementedException();
    }
}