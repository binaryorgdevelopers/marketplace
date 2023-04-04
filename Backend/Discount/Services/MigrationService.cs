using Npgsql;

namespace Discount.Services;

public static class MigrationService
{
    public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
    {
        var retryForAvailability = retry!.Value;

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();
        try
        {
            logger.LogInformation("Migrating Postgresql database ");
            using var connection =
                new NpgsqlConnection(configuration.GetValue<string>("ConnectionStrings:Postgres"));
            connection.Open();
            using var command = new NpgsqlCommand
            {
                Connection = connection
            };
            //Drops table
            // command.CommandText = "DROP TABLE IF EXISTS Coupon";
            // command.ExecuteNonQuery();
            //Create table
            command.CommandText =
                @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY , ProductName VARCHAR(100) NOT NULL,Description TEXT,AMOUNT INT)";
            command.ExecuteNonQuery();
            logger.LogInformation("Migrated postgresql database.");
        }
        catch (NpgsqlException e)
        {
            logger.LogError(e, "An error occured while migrating the postgresql database");
            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(host, retryForAvailability);
            }
        }

        return host;
    }
}