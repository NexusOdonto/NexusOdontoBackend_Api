namespace Domain.ValueObjects.Catalogs
{
    public sealed record ServiceDuration
    {
        //Propiedad
        public int Value { get; }

        //Constructor
        private ServiceDuration(int value)
        {
            Value = value;
        }

        //Create
        public static ServiceDuration Create(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("La duracion debe ser mayor que cero.", nameof(value));
            }

            if (value > 9999)
            {
                throw new ArgumentException("La duracion excede el maximo permitido.", nameof(value));
            }

            return new ServiceDuration(value);
        }

        public override string ToString() => Value.ToString();
    }
}
