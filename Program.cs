using JungleOlympics.Api.Data;
using JungleOlympics.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDbAsync();

app.MapAnimalsEndpoints();

app.Run();
