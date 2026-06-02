using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Categoria
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")]
    public int? CategoriaId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }

    [JsonPropertyName("protegida")]
    public bool Protegida { get; set; } 
}