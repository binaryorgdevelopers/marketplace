using Dapper;
using Npgsql;

namespace Ordering.Application.Queries;

public class OrderQueries : IOrderQueries
{
    private readonly string _connectionString;
    private readonly Guid _currentUserId;

    public OrderQueries(string connectionString, StateService stateService)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _currentUserId = stateService.CurrentUserId;
    }


    public async Task<Order> GetOrderAsync(Guid id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = connection.ExecuteMapperQuery<Order>(
            "SELECT o.\"Id\" AS ordernumber, o.\"OrderDate\" AS date, os.\"Name\" AS status, SUM(io.\"unitprice\") AS total " +
            "FROM ordering.orders AS o  " +
            "LEFT JOIN ordering.\"orderItems\" io ON o.\"Id\" = io.\"orderid\"   LEFT JOIN  ordering .orderstatus os ON o.\"BuyerId \"= os.\"Id \"" +
            $" WHERE os.\"IdentityGuid\" ='{id}' GROUP BY o.\"Id\", o.\"OrderDate\", os.\"Name\"  ORDER BY o.\"Id\"");
        if (result.Count == 0) throw new KeyNotFoundException();
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<OrderSummary>> GetOrderFromUserAsync(Guid? userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        string id = userId.ToString();

        var result = connection.ExecuteMapperQuery<OrderSummary>(
            "SELECT o.\"Id\" as ordernumber, o.\"OrderDate\" as \"date\", os.\"Name\" as \"status\", SUM(oi.\"Units\" * oi.\"UnitPrice\") as total " +
            " FROM \"ordering\".\"orders\" o " +
            " LEFT JOIN \"ordering\".\"orderItems\" oi ON o.\"Id\" = oi.\"OrderId\"" +
            " LEFT JOIN ordering.\"orderstatus\" os ON o.\"OrderStatusId\" = os.\"Id\"" +
            " LEFT JOIN \"ordering\".\"buyers\" ob ON o.\"BuyerId\" = ob.\"Id\"" +
            $" WHERE ob.\"IdentityGuid\" = '{userId}' GROUP BY o.\"Id\", o.\"OrderDate\", os.\"Name\" ORDER BY o.\"Id\"");
        return result.Any() ? result : Array.Empty<OrderSummary>();
    }

    public async Task<IEnumerable<CardType>> GetCardTypeAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection.ExecuteMapperQuery<CardType>("SELECT * FROM ordering.cardtypes");
    }
}

public abstract class A
{
    public abstract void ASD();
    public abstract string Ds();
}