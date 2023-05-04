using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Idempotency;

public class RequestManager:IRequestManager
{
    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CreateRequestCommandAsync<T>(Guid id)
    {
        throw new NotImplementedException();
    }
}