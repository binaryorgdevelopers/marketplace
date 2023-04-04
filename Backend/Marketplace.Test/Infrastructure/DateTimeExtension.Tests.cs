using Inventory.Domain.Extensions;

namespace Marketplace.Test.Infrastructure;

public class DateTimeExtensionTests
{
    [Fact]
    public void SetKindUtcNullInputTest()
    {
        DateTime? input = null;
        DateTime? result = input.SetKindUtc();
        Assert.Null(result);
    }

    [Fact]
    public void SetKindUtcNonNullRegularDateInput()
    {
        DateTime? input = DateTime.Now;
        DateTime? result = input.SetKindUtc();
        Assert.NotNull(result);
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
    }

    [Fact]
    public void UnspecifiedKindIsOverwritten()
    {
        DateTime? input = DateTime.Now;
        DateTime withKindInput = DateTime.SpecifyKind(input.Value, DateTimeKind.Unspecified);
        DateTime? result = withKindInput.SetKindUtc();
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
    }

    [Fact]
    public void LocalKindIsOverwritten()
    {
        DateTime? input = DateTime.Now;
        DateTime withKindUtcInput = DateTime.SpecifyKind(input.Value, DateTimeKind.Local);
        DateTime? result = withKindUtcInput.SetKindUtc();
        Assert.NotNull(result);
        /* note the behavior.  "DateTimeKind.Local" with overwritten with DateTimeKind.Utc */
        Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
    }
}