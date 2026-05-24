using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/ready");

app.MapPost("/fraud-score", (FraudScoreRequest request) =>
{
    var response = new FraudScoreResponse
    {
        Approved = true,
        FraudScore = 0.0,
    };
    return response;
});

app.Run();

