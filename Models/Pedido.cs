using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace frontendnet.Models;

public class Pedido
{
    [Display(Name = "Id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Display(Name = "Total")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    [JsonPropertyName("total")]
    
    public string? Total { get; set; } 

    [Display(Name = "Estado")]
    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [Display(Name = "Fecha de Creación")]
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Última Actualización")]
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("cliente")]
    public Usuario? Cliente { get; set; } 
}