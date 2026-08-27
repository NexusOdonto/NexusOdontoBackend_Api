namespace Domain.ValueObjects.Notifications
{
    public sealed record PhoneNumber
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private PhoneNumber(string value)
        {
            Value = value;
        }

        //Create
        public static PhoneNumber Create(string? value)
        {
            var phone = value?.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("El telefono es obligatorio.", nameof(value));
            }

            if (phone.Length > 30)
            {
                throw new ArgumentException("El telefono no puede superar 30 caracteres.", nameof(value));
            }

            if (!phone.All(c => char.IsDigit(c) || c == '+' || c == ' ' || c == '-'))
            {
                throw new ArgumentException("El telefono solo admite digitos, espacios, '+' y '-'.", nameof(value));
            }

            return new PhoneNumber(phone);
        }

        public override string ToString() => Value;
    }
}
