using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Carrito
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("clienteId")]
    public int ClienteId { get; set; }

    [JsonPropertyName("productos")]
    public List<ProductoCarrito>? Productos { get; set; }
}