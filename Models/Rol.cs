using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Rol
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")] 
    public int? Id { get; set; } 

    [Display(Name = "Nombre")]
    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }
}