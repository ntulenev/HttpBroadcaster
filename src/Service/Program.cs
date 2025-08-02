var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/hc");

app.MapPost("api/messages", async (
    Transport.InboundMessage message,
    CancellationToken ct) =>
{
    await Task.Delay(100, ct);
    return Results.Ok();
});

app.Run();
