
Console.WriteLine("Suma: " + Sumar(new Enumerar(10, 2)));
Imprimir(new Enumerar(10, 2));
Imprimir(new Invertir(10, 2));
void Imprimir(Enumerar e) {
    int count = 0;
    while (e.MoveNext()) {
        Console.WriteLine(e.Current);
        count++;
    }
    Console.WriteLine("Count: " + count);
}

int Sumar(Enumerar e) {
    int suma = 0;
    while (e.MoveNext()) {
        suma += e.Current;
    }
    return suma;
}

class Enumerar {
    int index;
    protected int max;
    protected int step;

    public Enumerar(int Max, int Step) {
        Current = 0;
        index = -1;
        max = Max;
        step = Step;
    }
    public int Current { get; protected set; }
    public virtual bool MoveNext() {
        if (Current < max) {
            Current += step;
            return true;
        }
        return false;
    }
}


class Invertir: Enumerar {
    public Invertir(int Max, int Step) : base(Max, Step) {
        Current = Max;
    }

    public override bool MoveNext() {
        if (Current > 0) {
            Current -= step;
            return true;
        }
        return false;
     }
}