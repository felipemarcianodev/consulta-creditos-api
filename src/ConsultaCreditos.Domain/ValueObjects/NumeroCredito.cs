namespace ConsultaCreditos.Domain.ValueObjects;

public sealed class NumeroCredito : IEquatable<NumeroCredito>
{
    public string Valor { get; }

    private NumeroCredito(string valor)
    {
        Valor = valor;
    }

    public static NumeroCredito Criar(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Número do crédito não pode ser vazio", nameof(valor));

        if (valor.Length > 50)
            throw new ArgumentException("Número do crédito não pode ter mais de 50 caracteres", nameof(valor));

        return new NumeroCredito(valor.Trim());
    }

    public bool Equals(NumeroCredito? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Valor == other.Valor;
    }

    public override bool Equals(object? obj)
    {
        return obj is NumeroCredito other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }

    public override string ToString()
    {
        return Valor;
    }

    public static implicit operator string(NumeroCredito numeroCredito)
    {
        return numeroCredito.Valor;
    }

    public static bool operator ==(NumeroCredito? left, NumeroCredito? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(NumeroCredito? left, NumeroCredito? right)
    {
        return !Equals(left, right);
    }
}
