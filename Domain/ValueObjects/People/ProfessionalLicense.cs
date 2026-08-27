using System.Timers;

namespace Domain.ValueObjects.People
{
    public sealed record ProfessionalLicense
    {
        //Propiedad
        public string Value { get; }

        //Constructor
        private ProfessionalLicense(string value)
        {
            Value = value;
        }

        //Create
        public static ProfessionalLicense Create(string value)
        {
            var license = value?.Trim(); // el value? es para controlar un create(null) y que no explote feito

            if (string.IsNullOrWhiteSpace(license))
            {
                throw new ArgumentException ("The value cannot null", nameof(value));
            }

            if (license.Length > 50)
            {
                throw new ArgumentException(" El valor no puede ser mayor a 50 ", nameof(value));
            }

            return new ProfessionalLicense(license);
        }

        public override string ToString() => Value;
    }
}