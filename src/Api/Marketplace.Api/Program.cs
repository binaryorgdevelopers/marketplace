using Marketplace.Api.Extensions;
using Marketplace.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterLambda();
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration,builder.Environment.IsDevelopment());


builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.UseErrorHandler();

app.MapControllers();

app.Run();