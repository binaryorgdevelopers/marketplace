namespace Ordering.Infrastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistsAsync();
    Task CreateRequestCommandAsync<T>(Guid id);
}