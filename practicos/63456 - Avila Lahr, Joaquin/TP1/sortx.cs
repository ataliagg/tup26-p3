using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
try
{
    var config = LeerArgumentos(args);

    if (config == null) return;

    var texto = LeerEntrada(config);
    var (filas, encabezado) = ParsearTexto(texto, config);
    var ordenado = OrdenarFilas(filas, config);
    var salida = ConvertirTexto(ordenado, encabezado, config);
    EscribirSalida(salida, config);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.Exit(1);
}

record CampoOrden(string Nombre, bool EsNumero, bool Desc);
record Configuracion( string? Entrada,string? Salida,string Delimitador,bool SinEncabezado,List<CampoOrden> Campos,bool Ayuda);