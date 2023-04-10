namespace Ordering.Infrastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistsAsync(Guid id);
    Task CreateRequestCommandAsync<T>(Guid id);
}