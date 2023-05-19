using Basket.Entities;
using Basket.Repositories;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Controllers;

[ApiController]
[Route("basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    public BasketController(IBasketRepository basketRepository, IHttpClientFactory httpClientFactory)
    {
        _basketRepository = basketRepository;
        _httpClientFactory = httpClientFactory;
    }

    [Authorize]
    [HttpGet("{username}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(Guid username)
        => Ok(await _basketRepository.GetBasketAsync(username.ToString()));

    [Authorize]
    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<ActionResult> Delete(Guid userName)
        => Ok(await _basketRepository.DeleteBasketAsync(userName.ToString()));

    [Authorize]
    [HttpPost(Name = "UpdateBasket")]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
    {
        var result = await _basketRepository.UpdateBasketAsync(basket);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var serverClient = _httpClientFactory.CreateClient();

        var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("http://localhost:1111/");

        var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = "basket",
            ClientSecret = "basket",
            Scope = "basket"
        });

        var apiClient = _httpClientFactory.CreateClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken);
        var response = await apiClient.GetAsync("http://localhost:1111/secret");
        var content = await response.Content.ReadAsStringAsync();

        return Ok(new
        {
            access_token = tokenResponse.AccessToken,
            message = content
        });
    }

    [Authorize]
    [HttpGet("check")]
    public ActionResult CheckAuth() => Ok(new
    {
        code = 200,
        message = "Api responding"
    });
}