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
    // Le agregamos la regla estricta para forzar el dominio (.com, .mx, etc.)
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "El correo debe tener un formato válido (ejemplo: usuario@dominio.com).")]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("nombre")]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [JsonPropertyName("rol")]
    public required string Rol { get; set; }
}