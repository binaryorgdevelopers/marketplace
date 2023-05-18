namespace Inventory.Api;

public record Health(
    string Host,
    string RootPath,
    string[] TimeStamp,
    TimeSpan Uptime
);