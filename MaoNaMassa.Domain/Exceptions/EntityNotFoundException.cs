namespace MaoNaMassa.Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, Guid id) 
        : base($"{entityName} com ID {id} não foi encontrado.")
    {
    }
}

