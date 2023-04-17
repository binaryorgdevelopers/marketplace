using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Ordering.Application.Extensions;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Polly;
using Polly.Retry;

namespace Ordering.Infrastructure;

public class OrderingContextSeed
{
    private IEnumerable<OrderStatus> GetOrderStatusFromFile(string contentRootPath, ILogger<OrderingContextSeed> log)
    {
        string csvFileOrderStatus = Path.Combine(contentRootPath, "Setup", "OrderStatus.csv");
        if (!File.Exists(csvFileOrderStatus))
        {
            return GetPredefinedOrderStatus();
        }

        string[] csvHeaders;
        try
        {
            string[] requiredHeaders = { "OrderStatus" };
            csvHeaders = GetHeaders(requiredHeaders, csvFileOrderStatus);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
            return GetPredefinedOrderStatus();
        }

        int id = 1;
        return File.ReadAllLines(csvFileOrderStatus)
            .Skip(1)
            .SelectTry(x => CreateOrderStatus(x, ref id))
            .OnCaughtException(ex =>
            {
                log.LogError(ex, $"EXCEPTION ERROR: {ex.Message}");
                return null;
            })
            .Where(x => x != null);
    }

    private OrderStatus CreateOrderStatus(string value, ref int id)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new Exception("Orderstatus is null or empty");
        }

        return new OrderStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
    }

    private IEnumerable<OrderStatus> GetPredefinedOrderStatus()
    {
        return new List<OrderStatus>()
        {
            OrderStatus.Submitted,
            OrderStatus.AwaitingValidation,
            OrderStatus.StockConfirmed,
            OrderStatus.Paid,
            OrderStatus.Shipped,
            OrderStatus.Cancelled
        };
    }

    private AsyncRetryPolicy CreatePolicy(ILogger<OrderingContextSeed> logger, string prefix, int retries = 3)
    {
        return Policy.Handle<SqlException>()
            .WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(10),
                onRetry: (exception, span, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry);
                }
            );
    }

    private string[] GetHeaders(string[] requiredHeaders, string csvFile)
    {
        string[] csvHeaders = File.ReadAllLines(csvFile).First().ToLowerInvariant().Split(',');
        if (csvHeaders.Length != requiredHeaders.Length)
        {
            throw new Exception(
                $"requiredHeader count '{requiredHeaders.Length}' is different from the read header '{csvHeaders.Length}'");
        }

        foreach (var requiredHeader in requiredHeaders)
        {
            if (!csvHeaders.Contains(requiredHeader))
            {
                throw new Exception($"does not contain required header '{requiredHeader}'");
            }
        }

        return csvHeaders;
    }
}