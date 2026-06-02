using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Bitacora
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")]
    public int? BitacoraId { get; set; }

    [Display(Name = "Acción")]
    [JsonPropertyName("accion")]
    public string? Accion { get; set; }

    [Display(Name = "Elemento")]
    [JsonPropertyName("elementoId")]
    
    public int? ElementoId { get; set; } 

    [Display(Name = "Dirección IP")]
    [JsonPropertyName("ip")]
    public string? IP { get; set; }

    [Display(Name = "Usuario")]
    [JsonPropertyName("usuario")]
    public string? Usuario { get; set; }

    [Display(Name = "Fecha")]
    [JsonPropertyName("fecha")]
    public DateTime? Fecha { get; set; }
}