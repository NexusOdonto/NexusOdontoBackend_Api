namespace Domain.ValueObjects.Chatbot
{
    public sealed record ChatIdentifier
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private ChatIdentifier(string value)
        {
            Value = value;
        }

        //Create
        public static ChatIdentifier Create(string? value)
        {
            var identifier = value?.Trim();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException("El identificador de chat es obligatorio.", nameof(value));
            }

            if (identifier.Length > 150)
            {
                throw new ArgumentException("El identificador no puede superar 150 caracteres.", nameof(value));
            }

            return new ChatIdentifier(identifier);
        }

        public override string ToString() => Value;
    }
}
