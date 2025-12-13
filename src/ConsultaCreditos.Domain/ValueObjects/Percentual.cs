namespace ConsultaCreditos.Domain.ValueObjects;

public sealed class Percentual : IEquatable<Percentual>, IComparable<Percentual>
{
    public decimal Valor { get; }

    private Percentual(decimal valor)
    {
        Valor = valor;
    }

    public static Percentual Criar(decimal valor)
    {
        if (valor < 0 || valor > 100)
            throw new ArgumentException("Percentual deve estar entre 0 e 100", nameof(valor));

        return new Percentual(Math.Round(valor, 2));
    }

    public static Percentual Zero => new(0);

    public decimal ParaDecimal()
    {
        return Valor / 100;
    }

    public Dinheiro AplicarSobre(Dinheiro valor)
    {
        return valor.Multiplicar(ParaDecimal());
    }

    public bool Equals(Percentual? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Valor == other.Valor;
    }

    public override bool Equals(object? obj)
    {
        return obj is Percentual other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }

    public int CompareTo(Percentual? other)
    {
        if (other is null) return 1;
        return Valor.CompareTo(other.Valor);
    }

    public override string ToString()
    {
        return $"{Valor:N2}%";
    }

    public static implicit operator decimal(Percentual percentual)
    {
        return percentual.Valor;
    }

    public static bool operator ==(Percentual? left, Percentual? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Percentual? left, Percentual? right)
    {
        return !Equals(left, right);
    }

    public static bool operator >(Percentual left, Percentual right)
    {
        return left.Valor > right.Valor;
    }

    public static bool operator <(Percentual left, Percentual right)
    {
        return left.Valor < right.Valor;
    }

    public static bool operator >=(Percentual left, Percentual right)
    {
        return left.Valor >= right.Valor;
    }

    public static bool operator <=(Percentual left, Percentual right)
    {
        return left.Valor <= right.Valor;
    }
}
