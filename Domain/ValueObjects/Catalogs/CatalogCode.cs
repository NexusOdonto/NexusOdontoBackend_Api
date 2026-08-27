namespace Domain.ValueObjects.Catalogs
{
    public sealed record CatalogCode
    {
        //Propiedad
        public string Value { get; }

        //Contructor
        private CatalogCode(string value)
        {
            Value = value;
        }

        //Create
        public static CatalogCode Create(string value)
        {
            var catalog = value?.Trim().ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(catalog))
            {
                throw new ArgumentException("The value cannot null", nameof(value));
            }

            if (catalog.Length > 50)
            {
                throw new ArgumentException(" El codigo no puede ser mayor a 50 ", nameof(value));
            }
            return new CatalogCode(catalog);
        }
        public override string ToString() => Value;

    }
}