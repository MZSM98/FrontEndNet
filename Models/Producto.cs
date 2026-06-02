using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Producto
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")] 
    public int? ProductoId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("titulo")]
    public required string Titulo { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("descripcion")]
    public required string Descripcion { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Precio")]
    [JsonPropertyName("precio")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)] 
    public decimal Precio { get; set; }

    [Display(Name = "Portada")]
    [JsonPropertyName("archivoId")]
    public int? ArchivoId { get; set; }

    
    [JsonPropertyName("categorias")]
    public List<Categoria>? Categorias { get; set; }
}