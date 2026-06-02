using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;


public class UsuarioPwd : Usuario 
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Display(Name = "Contraseña")]
    [DataType(DataType.Password)]
    
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", 
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial (ej. * o #).")]
    public required string Password { get; set; }
}