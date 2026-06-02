using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Archivo
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")] 
    public int? ArchivoId { get; set; }

    [Display(Name = "MIME")]
    [JsonPropertyName("mime")]
    public string? Mime { get; set; }

    [JsonPropertyName("nombre")]
    public string? Nombre { get; set; }

    [Display(Name = "Tamaño")]
    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [Display(Name = "Repositorio")]
    [JsonPropertyName("inDb")]
    public bool InDb { get; set; } = true;
}