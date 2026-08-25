namespace Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base("El recurso solicitado no fue encontrado.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string name, object key)
        : base($"El recurso '{name}' con clave ({key}) no fue encontrado.")
    {
    }
}
