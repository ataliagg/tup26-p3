abstract class Nodo {
    public abstract int Evaluar(int x = 0);
}
sealed class NumeroNodo : Nodo {
    int valor;
    public NumeroNodo(int valor) {
        this.valor = valor;
    }
    public override int Evaluar(int x = 0) => valor;
}
sealed class VariableNodo : Nodo {
    
}
sealed class NegativoNodo : Nodo
{
    
}
abstract class BinarioNodo : Nodo
{
    
}
sealed class SumaNodo : Nodo
{
    
}
sealed class RestaNodo : Nodo
{
    
}
sealed class MultiplicacionNodo : Nodo
{   
    
}
sealed class DivisionNodo : Nodo
{   
    
}