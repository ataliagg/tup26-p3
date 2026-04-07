
// sortx [input [output]] [-b|--by campo[:tipo[:orden]]]...
//       [-i|--input input] [-o|--output output]
//       [-d|--delimiter delimitador]
//       [-nh|--no-header] [-h|--help]

using System;
using System.Collections.Generic;
using System.IO;    
using System.Linq;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

record SortField(string Name,bool Numeric,bool descending);
record Appconfig(string ?InputFile, string ?OutputFile, char Delimiter, bool NoHeader, List<SortField> Fields);

var config=ParseArgs(args);
var text=ReadInput(config);
var (header,rows)=ParseDelimited(text,config.Delimiter,config.NoHeader);
SortRows(rows,config.Fields);
var output=Serialize(header,rows,config);
WriteOutput(output,config);

Appconfig ParseArgs(string [] a)
{
  string ?inputFile=null,outputFile=null;
  string delimiter=",";
   bool noHeader=false;
   var fields=new List<SortField>();

   for(int i = 0; i < a.Length; i++)
   {
      
      var x=a[i];
      if (x=="-i" || x == "--input") {if (++i>a.Length)
      Exiterror("Falta argumento para -i");
      inputFile=a[i];}
      else if (x=="-o" || x == "--output") {if (++i>a.Length)      
      Exiterror("Falta argumento para -o");
      outputFile=a[i];}
      else if (x=="-d" || x == "--delimiter") {if (++i>a.Length)
      Exiterror("Falta argumento para -d");
      delimiter=Unescape(a[i]); }
      
      else if (x=="-nh" || x == "--no-header"){noHeader=true;}
      else if (x=="-b" || x == "--by"){
      if (++i>a.Length) Exiterror("Falta argumento para -b");
      var p = a[i].Split(':');
      bool num=p.Length>1 && p[1].ToLowerInvariant()=="num";
      bool desc=p.Length>2 && p[2].ToLowerInvariant()=="desc";
      fields.Add(new SortField(p[0],num,desc));}
      else if(x=="-h" || x == "--help") {PrintHelp(); Environment.Exit(0);}
      else if (x.StartsWith("-")){
         Exiterror("opcion desconocida: "+x);
      }
      else {
         if(inputFile==null) inputFile=x;
         else if(outputFile==null) outputFile=x;
         else Exiterror("demasiados argumentos posicionales");
      }
    
   }
return new Appconfig(inputFile,outputFile,delimiter,noHeader,fields);
}
