namespace Inventory.Domain.Exceptions;

public class NullException
{
    public static void ThrowIfNull<T>(T entity)
    {
        if (entity is null) throw new Exception($"Value cant be null ,{nameof(entity)}");
    }
}