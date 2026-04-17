class Compilador {
    public static Nodo Parse(string expresion) { 
        // prepara token / cursor para recorrer la expresión
        return ParseExpresion();
    }

    Nodo ParseExpresion() {
        var nodo = ParseTermino();
        while (token == '+' || token == '-') {
            string operador = token;
            AvanzarToken();
            var terminoDerecho = ParseTermino();
            nodo = new NodoOperacion( nodo, operador, terminoDerecho);
        }
        return nodo;    
    }
    Nodo ParseTermino() {
        var nodo = ParseFactor();
        while (token == '*' || token == '/') {
            string operador = token;
            AvanzarToken();
            var terminoDerecho = ParseFactor();
            nodo = new NodoOperacion( operador, nodo, terminoDerecho);
        }
        return nodo;    
    }
       






}


/*
Expresion := Termino { ('+' | '-') Termino }
Termino   := Factor  { ('*' | '/') Factor }
Factor    := '+' Factor
          | '-' Factor
          | '(' Expresion ')'
          | numero
          | x

*/