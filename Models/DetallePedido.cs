using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class DetallePedido
{
    public int PedidoId { get; set; }
    
    public List<ProductoPedido> Productos { get; set; } = [];

    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Total { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }
}

public class ProductoPedido
{
    public int Id { get; set; }
    public string? Titulo { get; set; }

    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PrecioVenta { get; set; }
    
    public int Cantidad { get; set; }
    public int? ArchivoId { get; set; }
}

// Para usar en el PATCH del estado
public class UpdateEstadoPedido
{
    [Required]
    public string Estado { get; set; } = string.Empty;
}