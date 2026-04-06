using static system.Console;
using System;
using System.IO;
using System.Collections.Generic;


WriteLine(@"
Indicaciones para usar sortx:

sortx [archivoEntrada [archivoSalida]] -b campo[:tipo[:orden]]

Ejemplo:
sortx empleados.csv -b apellido
sortx empleados.csv -b edad:num:desc

Opciones:
  -b, --by            Campo de ordenamiento
  -i, --input         Archivo de entrada
  -o, --output        Archivo de salida
  -d, --delimiter     Delimitador (ej: , | \t)
  -nh, --no-header    Indica que no hay encabezado
  -h, --help          Mostrar ayuda
");

//MODELOS para configurar y ordenar

record SortField(string Name, bool Numeric, bool Descending);

record AppConfig(
    string? InputFile,
    string? OutputFile,
    string Delimiter,
    bool NoHeader,
    List<SortField> SortFields
);
