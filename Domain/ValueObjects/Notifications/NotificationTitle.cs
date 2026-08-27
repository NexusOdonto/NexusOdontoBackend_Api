namespace Domain.ValueObjects.Notifications
{
    public sealed record NotificationTitle
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private NotificationTitle(string value)
        {
            Value = value;
        }

        //Create
        public static NotificationTitle Create(string? value)
        {
            var title = value?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("El titulo es obligatorio.", nameof(value));
            }

            if (title.Length > 200)
            {
                throw new ArgumentException("El titulo no puede superar 200 caracteres.", nameof(value));
            }

            return new NotificationTitle(title);
        }

        public override string ToString() => Value;
    }
}
