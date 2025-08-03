using Abstractions;

using Logic;

using Microsoft.EntityFrameworkCore;

using Serilog;

using Storage;
using Storage.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services
    .AddOptions<UnitOfWorkConfiguration>()
    .Bind(builder.Configuration.GetSection("UnitOfWorkConfiguration"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<IBroadcastingHandler, BroadcastingHandler>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<MultiOutboxDbContext>>();
builder.Services.AddScoped<Func<Models.Environment, IOutboxWriter>>(sp =>
    env => ActivatorUtilities.CreateInstance<OutboxWriter>(sp, env));

builder.Services.AddDbContext<MultiOutboxDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MainDb");
    _ = options.UseNpgsql(connectionString);
});

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

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
