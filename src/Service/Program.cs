using Abstractions;

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
    IBroadcastingHandler handler,
    CancellationToken ct) =>
{

    using var _ = app.Logger.BeginScope("RequestId: {RequestId}", Guid.NewGuid());

    await handler.BroadcastAsync(Models.InboundMessage.CreateNew(message.Payload.ToString()), ct);

    app.Logger.LogInformation("Message processed successfully");

    return Results.Ok();
});

app.Run();
