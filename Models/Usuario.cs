using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Usuario
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")] 
    public int? Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} no es un correo válido.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "El correo debe tener un formato válido (ejemplo: usuario@dominio.com).")]
    [JsonPropertyName("email")]
    public string? Email { get; set; } // Quitamos "required string" y ponemos "string?"

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("rol")]
    public string? Rol { get; set; }
}