List<string> nombres = new List<string>();
nombres.Add("Adrián");
nombres.Add("María");
nombres.Add("Juan");

string[] nombres2 = new string[]{"Ana", "Luis", "Sofía"};
var nombres3 = ["Ana", "Luis", "Sofía"];
ListarNombres(nombres);
ListarNombres(nombres2);


void ListarNombres(IList<string> nombres) {
    for(var i = 0; i < nombres.Count; i++) {
        Console.WriteLine($"{i + 1}. {nombres[i]}");
    }
}

