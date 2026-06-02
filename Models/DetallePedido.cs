using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class DetallePedido
{
    [JsonPropertyName("pedidoId")]
    public int PedidoId { get; set; }

    [JsonPropertyName("total")]
    public string? Total { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("productos")]
    public List<ProductoPedido>? Productos { get; set; }
}

// Agregamos esta clase aquí mismo para leer la lista de cosas compradas
public class ProductoPedido
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("titulo")]
    public string? Titulo { get; set; }

    [JsonPropertyName("precioVenta")]
    public string? PrecioVenta { get; set; }

    [JsonPropertyName("cantidad")]
    public int Cantidad { get; set; }

    [JsonPropertyName("archivoId")]
    public int? ArchivoId { get; set; }
}