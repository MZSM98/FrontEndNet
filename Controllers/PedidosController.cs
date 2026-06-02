using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Controllers;

[Authorize(Roles = "Administrador")] 
public class PedidosController(PedidosClientService pedidos, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Pedido>? lista = [];
        try
        {
            ViewBag.SoloAdmin = true;
            lista = await pedidos.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception ex)
        {
            // Si algo falla, atrapamos el error y lo mandamos a la vista
            ViewBag.ErrorMessage = "No se pudieron cargar los pedidos. Detalle: " + ex.Message;
        }

        ViewBag.Url = configuration["UrlWebAPI"];
        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        DetallePedido? detalle = null;
        ViewBag.Url = configuration["UrlWebAPI"];
        try
        {
            ViewBag.SoloAdmin = true;
            detalle = await pedidos.GetDetalleAsync(id);
            if (detalle == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "No se pudo cargar el detalle. Detalle: " + ex.Message;
        }
        return View(detalle);
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarEstado(int id, string nuevoEstado)
    {
        try
        {
            await pedidos.UpdateEstadoAsync(id, nuevoEstado);
            return RedirectToAction(nameof(Detalle), new { id });
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
            
            TempData["ErrorMessage"] = "No se pudo actualizar el estado. Verifica que la transición sea válida.";
            return RedirectToAction(nameof(Detalle), new { id });
        }
    }
}