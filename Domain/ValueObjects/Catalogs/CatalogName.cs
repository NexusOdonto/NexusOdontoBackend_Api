namespace Domain.ValueObjects.Catalogs
{
    public sealed record CatalogName
    {
        //Propiedad
        public string Value { get; }


        //Constructor
        private CatalogName(string value)
        {
            Value = value;
        }
        // Create
        public static CatalogName Create(string value)
        {
            var name = value?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The name cannot be null", nameof(value));
            }
            if (name.Length > 80)
            {
                throw new ArgumentException("The name cannot be more characters than 50 ", nameof(value));
            }

            return new CatalogName(name);
        }
        public override string ToString() => Value;
    }
}