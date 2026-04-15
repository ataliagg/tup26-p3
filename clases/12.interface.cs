using System;
using System.Collections.Generic;

var c = new Caja();
c.Ingreso("Venta de producto A", 100);
c.Ingreso("Venta de producto B", 50);
c.Egreso("Compra de insumos", 30);
Console.WriteLine($"Saldo actual: ${c.Saldo}");
foreach (var movimiento in c.Detalle) {
    Console.WriteLine(movimiento);
}

enum TipoMovimiento {
    Ingreso,
    Egreso
}

interface IMovimiento {
    string Descripcion { get; }
    double Monto { get; }
    TipoMovimiento Tipo { get; }
}

interface ICaja {
    void Ingreso(string descripcion, double monto);
    void Egreso(string descripcion, double monto);
    double Saldo { get; }
    IList<IMovimiento> Detalle { get; }
}

class Movimiento : IMovimiento {
    public string Descripcion { get; private set; }
    public double Monto { get; private set; }
    public TipoMovimiento Tipo { get; private set; }

    public Movimiento(string descripcion, double monto, TipoMovimiento tipo) {
        if (string.IsNullOrWhiteSpace(descripcion)) {
            throw new ArgumentException("La descripción es requerida");
        }

        if (monto <= 0) {
            throw new ArgumentException("El monto debe ser positivo");
        }

        Descripcion = descripcion;
        Monto = monto;
        Tipo = tipo;
    }

    public override string ToString() {
        return $"{Tipo}: {Descripcion} - ${Monto}";
    }
}

class Caja : ICaja {
    private List<IMovimiento> movimientos;

    public Caja() {
        movimientos = new List<IMovimiento>();
    }

    public double Saldo {
        get {
            double saldo = 0;

            foreach (var movimiento in movimientos) {
                if (movimiento.Tipo == TipoMovimiento.Ingreso) {
                    saldo += movimiento.Monto;
                } else {
                    saldo -= movimiento.Monto;
                }
            }

            return saldo;
        }
    }

    public IList<IMovimiento> Detalle {
        get {
            return new List<IMovimiento>(movimientos);
        }
    }

    public void Ingreso(string descripcion, double monto) {
        var movimiento = new Movimiento(descripcion, monto, TipoMovimiento.Ingreso);
        movimientos.Add(movimiento);
    }

    public void Egreso(string descripcion, double monto) {
        if (monto > Saldo) {
            throw new InvalidOperationException("No hay saldo suficiente para registrar el egreso");
        }

        var movimiento = new Movimiento(descripcion, monto, TipoMovimiento.Egreso);
        movimientos.Add(movimiento);
    }
}
