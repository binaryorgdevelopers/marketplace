using Dapper;
using Discount.gRPC.Protos;
using Npgsql;

namespace Discount.Repositories;

public class DiscountRepository : IDiscountRepository
{
    public DiscountRepository(IConfiguration configuration)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    private IConfiguration Configuration { get; }

    public async Task<bool> CreateDiscount(CouponModel coupon)
    {
        await using var connection =
            new NpgsqlConnection(Configuration.GetValue<string>("ConnectionStrings:Postgres"));
        var affected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName,Description,Amount) Values (@ProductName,@Description,@Amount)",
            new { coupon.ProductName, coupon.Description, coupon.Amount });
        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(Guid productId)
    {
        await using var connection =
            new NpgsqlConnection(Configuration.GetValue<string>("ConnectionStrings:Postgres"));
        var result = await connection.ExecuteAsync("DELETE FROM Coupon WHERE productId=@productId",
            new { productId });
        return result >= 0;
    }

    public async Task<CouponModel> GetDiscount(Guid productId)
    {
        await using var connection =
            new NpgsqlConnection(Configuration.GetValue<string>("ConnectionStrings:Postgres"));
        var coupon = await connection.QueryFirstOrDefaultAsync<CouponModel>
            ("Select * from Coupon WHERE Id=@ProductId", new { ProductId = productId });
        return coupon ?? new CouponModel
            { ProductName = "No Discount", Amount = 0, Description = "No Discount description" };
    }

    public async Task<bool> UpdateDiscount(CouponModel coupon)
    {
        await using var connection =
            new NpgsqlConnection(Configuration.GetValue<string>("ConnectionStrings:Postgres"));

        var result = await connection.ExecuteAsync(
            "UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount",
            new { coupon.ProductName, coupon.Description, coupon.Amount }
        );
        return result > 0;
    }
}