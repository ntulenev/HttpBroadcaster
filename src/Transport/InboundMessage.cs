using System.Text.Json;
using System.Text.Json.Serialization;

namespace Transport;

public sealed class InboundMessage
{
    [JsonPropertyName("payload")]
    public required JsonElement Payload { get; init; }
}
