using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class UpdateEstadoPedido
{
    [JsonPropertyName("estado")]
    public required string Estado { get; set; }
}