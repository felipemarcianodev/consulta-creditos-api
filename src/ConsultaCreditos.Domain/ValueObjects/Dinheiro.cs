namespace ConsultaCreditos.Domain.ValueObjects;

public sealed class Dinheiro : IEquatable<Dinheiro>, IComparable<Dinheiro>
{
    public decimal Valor { get; }

    private Dinheiro(decimal valor)
    {
        Valor = valor;
    }

    public static Dinheiro Criar(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));

        return new Dinheiro(Math.Round(valor, 2));
    }

    public static Dinheiro Zero => new(0);

    public Dinheiro Somar(Dinheiro outro)
    {
        return Criar(Valor + outro.Valor);
    }

    public Dinheiro Subtrair(Dinheiro outro)
    {
        return Criar(Valor - outro.Valor);
    }

    public Dinheiro Multiplicar(decimal fator)
    {
        return Criar(Valor * fator);
    }

    public Dinheiro Dividir(decimal divisor)
    {
        if (divisor == 0)
            throw new ArgumentException("Divisor não pode ser zero", nameof(divisor));

        return Criar(Valor / divisor);
    }

    public bool Equals(Dinheiro? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Valor == other.Valor;
    }

    public override bool Equals(object? obj)
    {
        return obj is Dinheiro other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }

    public int CompareTo(Dinheiro? other)
    {
        if (other is null) return 1;
        return Valor.CompareTo(other.Valor);
    }

    public override string ToString()
    {
        return Valor.ToString("N2");
    }

    public static implicit operator decimal(Dinheiro dinheiro)
    {
        return dinheiro.Valor;
    }

    public static bool operator ==(Dinheiro? left, Dinheiro? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Dinheiro? left, Dinheiro? right)
    {
        return !Equals(left, right);
    }

    public static bool operator >(Dinheiro left, Dinheiro right)
    {
        return left.Valor > right.Valor;
    }

    public static bool operator <(Dinheiro left, Dinheiro right)
    {
        return left.Valor < right.Valor;
    }

    public static bool operator >=(Dinheiro left, Dinheiro right)
    {
        return left.Valor >= right.Valor;
    }

    public static bool operator <=(Dinheiro left, Dinheiro right)
    {
        return left.Valor <= right.Valor;
    }

    public static Dinheiro operator +(Dinheiro left, Dinheiro right)
    {
        return left.Somar(right);
    }

    public static Dinheiro operator -(Dinheiro left, Dinheiro right)
    {
        return left.Subtrair(right);
    }

    public static Dinheiro operator *(Dinheiro dinheiro, decimal fator)
    {
        return dinheiro.Multiplicar(fator);
    }

    public static Dinheiro operator /(Dinheiro dinheiro, decimal divisor)
    {
        return dinheiro.Dividir(divisor);
    }
}
