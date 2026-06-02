using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class ProductoCarrito
{
    [JsonPropertyName("producto")]
    public Producto? Producto { get; set; }

    [JsonPropertyName("cantidad")]
    public int Cantidad { get; set; }
}