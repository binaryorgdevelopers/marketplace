using System.Data.SqlClient;
using Dapper;

namespace Ordering.Application.Queries;

public class OrderQueries : IOrderQueries
{
    private string _connectionString = string.Empty;

    public OrderQueries(string connectionString)
        => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));


    public async Task<Order> GetOrderAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var result = connection.ExecuteMapperQuery<Order>(
            @"select  o.[Id] AS ordernumber,o.OrderDate AS date,o.Description AS description, o.Address_City AS city,o.Address_Country AS country,
                o.Address_State AS state,o.Address_Street AS street, o.Address_ZipCode AS zipcode,os.Name AS status,io.ProductName AS productname,
                oi.Units AS units,io.UnitPrice AS unitprice,io.PictureUrl AS pictureurl
                FROM ordering.Orders o
                LEFT JOIN ordering.OrderItems io ON o.Id=io.orderid
                LEFT JOIN ordering.orderstatus os ON o.OrderStatusId=os.Id
                WHERE o.Id=@id", new { id });
        if (result.Count == 0) throw new KeyNotFoundException();
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<OrderSummary>> GetOrderFromUserAsync(Guid userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection.ExecuteMapperQuery<OrderSummary>(
            @"SELECT o.[Id] AS ordernumber,o.[OrderDate] AS [date], os.[Name] AS [status],SUM(io.unitprice) AS total
            FROM [ordering].[orders]  o
            LEFT JOIN[ordering].[orderitems] io ON o.Id=io.orderid
            LEFT JOIN[ordering].[orderstatus] ob ON o.BuyerId=ob.Id
            WHERE ob.IdentityGuid=@userId
            GROUP BY o.[Id],o.[OrderDate],os.[Name]
            ORDER BY o.[Id] ", new { userId });
    }

    public async Task<IEnumerable<CardType>> GetCardTypeAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection.ExecuteMapperQuery<CardType>("SELECT * FROM ordering.cardtypes");
    }
}