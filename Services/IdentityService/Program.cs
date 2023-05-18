using IdentityService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddIdentityServer()
    .AddInMemoryApiResources(Configuration.GetApis())
    .AddInMemoryClients(Configuration.GetClients())
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.MapGet("/", () => "Identity Service");

app.UseRouting();

app.UseIdentityServer();

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();