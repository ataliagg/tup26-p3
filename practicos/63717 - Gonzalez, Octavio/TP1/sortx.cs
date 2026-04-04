
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

using System;

//ignorar comentario... Record: es como una clase peero mas corta. 

//record Empleado (int IDempleado, string nombre);  Estos valores no suelen cambiar, solo son de lectura. Si hay dos con los mismos datos, C# los considera iguales 





Console.WriteLine($"sortx {string.Join(" ", args)}");