namespace Domain.ValueObjects.Chatbot
{
    public sealed record TicketTitle
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private TicketTitle(string value)
        {
            Value = value;
        }

        //Create
        public static TicketTitle Create(string? value)
        {
            var title = value?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("El titulo del ticket es obligatorio.", nameof(value));
            }

            if (title.Length > 200)
            {
                throw new ArgumentException("El titulo no puede superar 200 caracteres.", nameof(value));
            }

            return new TicketTitle(title);
        }

        public override string ToString() => Value;
    }
}
