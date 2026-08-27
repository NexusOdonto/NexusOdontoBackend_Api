namespace Domain.ValueObjects.Catalogs
{
    public sealed record Price
    {
        //Propiedad
        public decimal Value { get; }

        //Constructor
        private Price(decimal value)
        {
            Value = value;
        }

        //Create
        public static Price Create(decimal value)
        {
            var amount = Math.Round(value, 2);

            if (amount < 0)
            {
                throw new ArgumentException("El precio no puede ser negativo.", nameof(value));
            }

            if (amount > 9999999999.99m)
            {
                throw new ArgumentException("El precio excede el maximo permitido.", nameof(value));
            }

            return new Price(amount);
        }

        public override string ToString() => Value.ToString("0.00");
    }
}
