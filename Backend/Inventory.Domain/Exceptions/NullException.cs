namespace Inventory.Domain.Exceptions;

public class NullException
{
    public static void ThrowIfNull<T>(T entity) => throw new Exception($"Value cant be null ,{nameof(entity)}");
}