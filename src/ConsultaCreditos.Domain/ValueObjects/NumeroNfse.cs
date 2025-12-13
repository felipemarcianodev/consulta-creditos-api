namespace ConsultaCreditos.Domain.ValueObjects;

public sealed class NumeroNfse : IEquatable<NumeroNfse>
{
    public string Valor { get; }

    private NumeroNfse(string valor)
    {
        Valor = valor;
    }

    public static NumeroNfse Criar(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Número da NFS-e não pode ser vazio", nameof(valor));

        if (valor.Length > 50)
            throw new ArgumentException("Número da NFS-e não pode ter mais de 50 caracteres", nameof(valor));

        return new NumeroNfse(valor.Trim());
    }

    public bool Equals(NumeroNfse? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Valor == other.Valor;
    }

    public override bool Equals(object? obj)
    {
        return obj is NumeroNfse other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Valor.GetHashCode();
    }

    public override string ToString()
    {
        return Valor;
    }

    public static implicit operator string(NumeroNfse numeroNfse)
    {
        return numeroNfse.Valor;
    }

    public static bool operator ==(NumeroNfse? left, NumeroNfse? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(NumeroNfse? left, NumeroNfse? right)
    {
        return !Equals(left, right);
    }
}
