var e = new EnumerarRango(10);
var l = new EnumerarLista(new List<int> { 10, 20, 30 });
var e2 = new EnumerarRango(5);

Listar(e);
Listar(l);
Listar(e2);
Listar(new EnumerarDias());

void Listar(Enumerar e) {
    Console.WriteLine("Enumerar:");
    while (e.MoveNext()) {
        Console.WriteLine(e.Current);
    }
}



class Enumerar<T>
{
    public virtual T Current { get; protected set; }
    public virtual bool MoveNext() {
        return false;   
    }
}
class EnumerarLista: Enumerar<int>
{
    List<int> enteros;
    int index;
    public EnumerarLista(List<int> enteros)
    {
        this.enteros = enteros;
        index = -1;
    }

    public override int Current => enteros[index];
    public override bool MoveNext()
    {
        index++;
        return index < enteros.Count;
     }
    }

class EnumerarRango : Enumerar<int>
{
    int current;
    int max;
    
    public EnumerarRango(int Max) {
        current = 0;
        max = Max;
    }
    public override int Current => current; 
    public override bool MoveNext() {
        if (current < max) {
            current += 1;
            return true;
        }
        return false;
     }
}

class EnumerarDias : Enumerar<string>
{
    int current;
    string[] dias = new string[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
    public EnumerarDias() {
        current = 0;
    }
    public override string Current => dias[current]; 
    public override bool MoveNext() {
        if (current < 7) {
            current += 1;
            return true;
        }
        return false;
     }
}