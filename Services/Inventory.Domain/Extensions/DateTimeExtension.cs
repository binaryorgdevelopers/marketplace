namespace Inventory.Domain.Extensions;

public static class DateTimeExtension
{
    public static DateTime? SetKindUtc(this DateTime? dateTime)
        => dateTime?.SetKindUtc();


    public static DateTime SetKindUtc(this DateTime dateTime)
        => dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
}