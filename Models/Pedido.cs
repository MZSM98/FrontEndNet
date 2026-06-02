using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Pedido
{
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Display(Name = "Total")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Total { get; set; }

    [Display(Name = "Estado")]
    public string Estado { get; set; } = "pendiente";

    [Display(Name = "Fecha de Creación")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Última Actualización")]
    public DateTime UpdatedAt { get; set; }

    public Usuario? Cliente { get; set; } 
}