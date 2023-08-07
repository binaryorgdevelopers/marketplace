using Identity.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Models;
using Xunit.Abstractions;

namespace Identity.Test.Services;

public class TokenProviderTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Mock<IOptions<JwtOptions>> _mockJwtOptions;
    private readonly TokenProvider _tokenProvider;

    public TokenProviderTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mockJwtOptions = new Mock<IOptions<JwtOptions>>();
        var jwtOptions = new JwtOptions()
        {
            Secret = "dotnet_identity_service_test",
            ExpiryMinutes = 240,
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidAudience = "ValidAudience"
        };
        _mockJwtOptions.Setup(x => x.Value).Returns(jwtOptions);
        _tokenProvider = new TokenProvider(_mockJwtOptions.Object);
    }

    [Theory]
    [InlineData("dilshod@gmail.com", "+9989975412642", "Dilshodbek", "Hamidov", "Admin")]
    [InlineData("", "+9989975412642", "Dilshodbek", "Hamidov", "Admin")]
    [InlineData("khamidov@gmail.com", "+9989975412642", "Dilshodbek", "Hamidov", "")]
    public async Task ShouldGenerateTokenWithGivenCredentials(string email, string phoneNumber, string firstName,
        string lastName, string role)
    {
        var tokenRequest = new TokenRequest(Guid.NewGuid(), email, phoneNumber, firstName, lastName, role);

        var token = await _tokenProvider.GenerateToken(tokenRequest);

        Assert.NotNull(token.Token);
        Assert.NotNull(token.ExpiresIn);
    }

    [Theory]
    [InlineData(
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjkxOTU3N2NjLTAwZDktNDQ3ZC1hYjI5LWMyMjBkMzdjZDhkYiIsIm5iZiI6MTY5MTM4NDgyMCwiZXhwIjoxNjkxMzk5MjIwLCJpYXQiOjE2OTEzODQ4MjB9.A2KESGmAp1iFylKLRe2yhZ2remjD13v2YHtT6QBxXls")]
    public void ValidateJwtToken_Should_Return_UserId(string token)
    {
        var result = _tokenProvider.ValidateJwtToken(token);
        _testOutputHelper.WriteLine(result.ToString());
        Assert.False(result.HasValue);
    }
}