using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Registro
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(255, ErrorMessage = "El nombre no puede exceder los 255 caracteres.")]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} no es un correo válido.")]
    [StringLength(255, ErrorMessage = "El correo no puede exceder los 255 caracteres.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos ocho caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
        ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un símbolo.")]
    public required string Password { get; set; }
}