class Compilador {
    char[] tokens (string expresion) {
        // Implementar tokenización aquí
    }
    tekenizador Tokenizar(string expresion) {
        // Implementar tokenización aquí
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
    Nodo ParseFactor(){

        if (token == '+') {
            AvanzarToken();
            return ParseFactor();
        }
        if (token == '-') {
            AvanzarToken();
            return new NodoOperacion(new NodoValor(0), "-", ParseFactor());
        }
        if (token == '(') {
            AvanzarToken();
            var nodo = ParseExpresion();
            if (token == ')') {
                AvanzarToken();
                return nodo;
            } else 
                throw new Exception("Se esperaba ')'");
        }
        if (EsNumero(token)) {
            var valor = int.Parse(token);
            AvanzarToken();
            return new NodoValor(valor);
        }
        if (token == 'x' || token == 'X') {
            AvanzarToken();
            return new NodoVariable();
        }
        throw new Exception("Token inesperado: " + token);
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