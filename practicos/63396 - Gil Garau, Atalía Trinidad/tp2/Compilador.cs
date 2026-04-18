class Compilador {

    enum TipoToken {
        Numero, Variable,
        Suma, Resta,
        Multiplicacion, Division,
        ParentesisAbierto, ParentesisCerrado,
        Final
    }
    record Token(TipoToken Tipo, string Valor = "");

    List<Token> Tokenizar(string expresion) {
        int posicion = 0;
        char continuar() => expresion[posicion++];

        List<Token> token = new();

        while (posicion < expresion.Length) {
            char c = expresion[posicion];

            if (char.IsWhiteSpace(c)) {
             continuar();
                continue;
            }

           if (char.IsDigit(c)) {
                string numero = "";
                while (posicion < expresion.Length && char.IsDigit(expresion[posicion])) {
                    numero += continuar();
                }
                token.Add(new Token(TipoToken.Numero, numero));
                continue;
            }

            if (c == 'x' || c == 'X') {
                token.Add(new Token(TipoToken.Variable, c.ToString()));
                continuar();
                continue;
            }

            switch (c) {
                case '+':
                    token.Add(new Token(TipoToken.Suma));
                    break;
                case '-':
                    token.Add(new Token(TipoToken.Resta));
                    break;
                case '*':
                    token.Add(new Token(TipoToken.Multiplicacion));
                    break;
                case '/':
                    token.Add(new Token(TipoToken.Division));
                    break;
                case '(':
                    token.Add(new Token(TipoToken.ParentesisAbierto));
                    break;
                case ')':
                    token.Add(new Token(TipoToken.ParentesisCerrado));
                    break;
                default:
                    throw new Exception($"Ingreso carácter inesperado: {c}");
            }

            continuar();
        }

        token.Add(new Token(TipoToken.Final));
        return token;
    }

    // public static Nodo Parse(string expresion) {
    //  throw new NotImplementedException("Implementar el parser para convertir la expresión en un AST.");
    //}
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
