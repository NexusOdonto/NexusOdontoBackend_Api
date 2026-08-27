
namespace Domain.ValueObjects.People
{
    public sealed record HireDate
    {

        // Propiedad
        public DateTime Value { get; }

        //Constructor    
        private HireDate(DateTime dateTime)
        {
            Value = dateTime;
        }

        // metodo Create
        public static HireDate Create(DateTime value)
        {
            var date = value.Date;

            if (date > DateTime.UtcNow.Date)
            {
                throw new ArgumentException("The date of register cannot be later than the currently hour", nameof(value)); // Sirve para que la excepción diga cuál argumento estuvo mal, y para que si algún día renombras el parámetro, el string se actualice solo. Con "value" escrito a mano, el renombre lo dejaría desactualizado en silencio.
            }

            return new HireDate(date);
        }


    }


}