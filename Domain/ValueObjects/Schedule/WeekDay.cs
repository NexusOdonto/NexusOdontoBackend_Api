namespace Domain.ValueObjects.Schedule
{
    public sealed record WeekDay
    {
        //Propiedad
        public int Value { get; }

        //Constructor
        private WeekDay(int value)
        {
            Value = value;
        }

        //Create
        public static WeekDay Create(int value)
        {
            if (value < 0 || value > 6)
            {
                throw new ArgumentException("El dia de la semana debe estar entre 0 y 6.", nameof(value));
            }

            return new WeekDay(value);
        }

        public override string ToString() => Value.ToString();
    }
}
