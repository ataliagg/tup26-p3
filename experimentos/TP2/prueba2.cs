

Valor Evaluar(string expresion) {
    bool IsEnd()   => pos >= expresion.Length;
    char Next()    => !IsEnd() ? expresion[pos] : '\0';
    char Consume() => !IsEnd() ? expresion[pos++] : '\0';

    Nodo Expresion() {
        var resultado = Termino();
        while (Next() == "+" || Next() == "-") {
            char operador = Next()[0];
            Consume();
            var derecho = Termino();
            resultado = new BinarioNodo(operador, resultado, derecho);
        }
        return resultado;
    }

    Nodo Termino() {
        var resultado = Factor();
        while (Next() == "*" || Next() == "/") {
            char operador = Next()[0];
            Consume();
            var derecho = Factor();
            resultado = new BinarioNodo(operador, resultado, derecho);
        }
        return resultado;
    }

    Nodo Unario() {
        if (Next() == "+" || Next() == "-") {
            char operador = Next()[0];
            Consume();
            var operando = Unario();
            return operador == '+' ? operando : new UnarioNodo(operador, operando);
        }
        return Factor();
    }

    Nodo Primario() {
         if(Next() == "(") {
            Consume();
            var resultado = Expresion();
            if(Next() != ")") {
                throw new Exception("Se esperaba ')'");
            }
            Consume();
            return resultado;
        } else if (IsNumber(Next().ToString())) {
            var number = "";
            while (IsNumber(Next().ToString()) || Next() == '.') {
                number += Consume();
            }
            return new NumeroNodo(double.Parse(number));
        } else if (IsDireccion(Next().ToString())) {
            var direccion = "";
            while (IsDireccion(Next().ToString())) {
                direccion += Consume();
            }
            return new DireccionNodo(direccion, 0, 0); // Fila y columna son placeholders
        } else if (IsString(Next().ToString())) {
            Consume(); // Consumir la comilla inicial
            var str = "";
            while (!IsEnd() && Next() != '"') {
                str += Consume();
            }
            if (Next() != '"') {
                throw new Exception("Se esperaba '\"'");
            }
            Consume(); // Consumir la comilla final
            return new CadenaNodo(str);
        } else if (IsIdentifier(Next().ToString())) {
            var identifier = "";
            while (IsIdentifier(Next().ToString())) {
                identifier += Consume();
            }
            return new DireccionNodo(identifier, 0, 0); // Fila y columna son placeholders
        } else {
            throw new Exception($"Token inesperado: {Next()}");
        }
    }

    bool IsFormule() => Next() == "="; 
    bool IsCadena()  => Next() == "\""; 
    bool IsNumero()  => IsNumber(Next().ToString()); 

    if (IsFormule()) {
        Consume();
        return Expresion();
    } 
    if (IsCadena()) {
        return CadenaNodo(expresion.Trim('"'));
    } 
    if (IsNumero()) {
        return NumeroNodo(double.Parse(expresion));
    }
    throw new Exception("Expresión no válida");
}

record Nodo{}
record NumeroNodo(double Valor) : Nodo {}
record CadenaNodo(string Valor) : Nodo {}

record UnarioNodo(string operador, Nodo operando) : Nodo { }
record BinarioNodo(string operador, Nodo izquierdo, Nodo derecho) : Nodo {}
record DireccionNodo( string Nombre, int Fila, int Columna) : Nodo {}

record FormulaNodo(string Nombre, List<Nodo> Argumentos) : Nodo { }
record DireccionNodo(string Nombre, int Fila, int Columna) : Nodo { }
record Valor(double Valor, string Texto, Nodo Expresion) { }
