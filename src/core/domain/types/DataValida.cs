
namespace core.domain.types
{
   internal struct DataValida : IEquatable<DataValida>
{
    static readonly DateTime DATA_MINIMA = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    private DateTime valor;

    internal DateTime Valor
    {
        readonly get { return valor; }
        set
        {
            ValidarValor(value);
            valor = value;
        }
    }

    internal DataValida(DateTime valor)
    {
        Valor = valor;
    }

    public static implicit operator DataValida(DateTime value)
    {
        return new DataValida(value);
    }

    public static implicit operator DateTime(DataValida valor)
    {
        return valor.Valor;
    }

    public override readonly string ToString()
           => Valor.ToString();

    private static void ValidarValor(DateTime value)
    {
        if (value < DATA_MINIMA)
        {
            throw new ArgumentException("Não é permitido criar pedidos com data anterior a 01/01/2024.");
        }

        if (value > DateTime.Now)
        {
            throw new ArgumentException("Não é permitido criar pedidos com data/horário futuro.");
        }
    }

    public bool Equals(DataValida other)
    {
        return this.Valor == other.Valor;
    }
}

}