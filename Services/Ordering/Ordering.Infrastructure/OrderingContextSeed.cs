using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Extensions;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using Polly;
using Polly.Retry;

namespace Ordering.Infrastructure;

public class OrderingContextSeed
{
    public async Task SeedAsync(OrderingContext context, IWebHostEnvironment env, IOptions<OrderingSettings> settings,
        ILogger<OrderingContextSeed> logger)
    {
        var policy = CreatePolicy(logger, nameof(OrderingContextSeed));

        await policy.ExecuteAsync(async () =>
        {
            var useCustomizationData = settings.Value.UseCustomizationData;
            var contentRootPath = env.ContentRootPath;

            var c = GetCardTypesFromFile(contentRootPath, logger);
            await using (context)
            {
                await context.Database.MigrateAsync();
                if (!context.CardTypes.Any())
                {
                    context.CardTypes.AddRange(useCustomizationData
                        ? GetCardTypesFromFile(contentRootPath, logger)
                        : GetPredefinedCardTypes());
                    await context.SaveChangesAsync();
                }

                if (!context.OrderStatus.Any())
                {
                    context.OrderStatus.AddRange(useCustomizationData
                        ? GetOrderStatusFromFile(contentRootPath, logger)
                        : GetPredefinedOrderStatus());
                }

                await context.SaveChangesAsync();
            }
        });
    }


    private IEnumerable<CardType> GetCardTypesFromFile(string contentRootPath, ILogger<OrderingContextSeed> log)
    {
        string csvFileCardTypes = Path.Combine(contentRootPath, "Setup", "CardTypes.csv");

        if (!File.Exists(csvFileCardTypes))
        {
            return GetPredefinedCardTypes();
        }

        string[] csvheaders;
        try
        {
            string[] requiredHeaders = { "CardType" };
            csvheaders = GetHeaders(requiredHeaders, csvFileCardTypes);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
            return GetPredefinedCardTypes();
        }

        int id = 1;
        return File.ReadAllLines(csvFileCardTypes)
            .Skip(1) // skip header column
            .SelectTry(x => CreateCardType(x, ref id))
            .OnCaughtException(ex =>
            {
                log.LogError(ex, "EXCEPTION ERROR: {Message}", ex.Message);
                return null;
            })
            .Where(x => x != null);
    }

    private IEnumerable<CardType> GetPredefinedCardTypes()
    {
        return Enumeration.GetAll<CardType>();
    }

    private CardType CreateCardType(string value, ref int id)
    {
        if (String.IsNullOrEmpty(value))
        {
            throw new Exception("Orderstatus is null or empty");
        }

        return new CardType(Guid.NewGuid(), value.Trim('"').Trim());
    }


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

        return new OrderStatus(Guid.NewGuid(), value.Trim('"').Trim().ToLowerInvariant());
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