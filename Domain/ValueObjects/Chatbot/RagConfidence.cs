namespace Domain.ValueObjects.Chatbot
{
    public sealed record RagConfidence
    {
        //Propiedad
        public decimal Value { get; }

        //Constructor
        private RagConfidence(decimal value)
        {
            Value = value;
        }

        //Create
        public static RagConfidence Create(decimal value)
        {
            var confidence = Math.Round(value, 4);

            if (confidence < 0 || confidence > 1)
            {
                throw new ArgumentException("La confianza debe estar entre 0 y 1.", nameof(value));
            }

            return new RagConfidence(confidence);
        }

        public override string ToString() => Value.ToString("0.0000");
    }
}
