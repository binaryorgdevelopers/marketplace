namespace Ordering.Domain.Abstractions;

public interface IRequestManager
{
    Task<bool> ExistsAsync(Guid id);
    Task CreateRequestCommandAsync<T>(Guid id);
}