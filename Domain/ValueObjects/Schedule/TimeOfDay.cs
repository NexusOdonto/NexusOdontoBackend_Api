namespace Domain.ValueObjects.Schedule
{
    public sealed record TimeOfDay
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private TimeOfDay(string value)
        {
            Value = value;
        }

        //Create
        public static TimeOfDay Create(string? value)
        {
            var time = value?.Trim();

            if (string.IsNullOrWhiteSpace(time))
            {
                throw new ArgumentException("La hora es obligatoria.", nameof(value));
            }

            if (!TimeOnly.TryParseExact(time, "HH:mm", out _))
            {
                throw new ArgumentException("La hora debe tener el formato HH:mm.", nameof(value));
            }

            return new TimeOfDay(time);
        }

        public override string ToString() => Value;
    }
}
