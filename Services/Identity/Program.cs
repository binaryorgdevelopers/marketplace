using Identity;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServices()
    .AddRepositories()
    .AddCustomDbContext();
builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers();

builder.Services
    .AddIdentityServer()
    .AddInMemoryApiScopes(Configuration.GetApis())
    .AddInMemoryClients(Configuration.GetClients())
    .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
    .AddDeveloperSigningCredential();


var app = builder.Build();

app.UseIdentityServer();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();