using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Controllers;

[Authorize] // Permite acceso tanto a Administrador como a Cliente
public class PedidosController(PedidosClientService pedidos, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Pedido>? lista = [];
        try
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol == "Administrador")
            {
                ViewBag.SoloAdmin = true;
                lista = await pedidos.GetAsync(); // Llama a getAll() del backend
            }
            else
            {
                // Cambiamos a ClaimTypes.Name porque nuestro identificador real es el Email
                var userEmail = User.FindFirstValue(ClaimTypes.Name) ?? "";
                
                // Llama a getAllDelCliente() usando el correo
                lista = await pedidos.GetClienteAsync(userEmail); 
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
                
            // Si el cliente aún no tiene pedidos y el backend arroja 404, mostramos la lista vacía
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                return View(lista);
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
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol == "Administrador")
            {
                ViewBag.SoloAdmin = true;
                detalle = await pedidos.GetDetalleAsync(id); // Admin
            }
            else
            {
                detalle = await pedidos.GetDetalleClienteAsync(id); // Cliente
            }
            
            if (detalle == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
                
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound || ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return NotFound();
        }
        return View(detalle);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")] // Solo el admin puede cambiar el estado, tal como en tu pedido_routes.ts
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
            
            // Si el backend devuelve 422 por transición inválida
            TempData["ErrorMessage"] = "No se pudo actualizar el estado. Verifica que la transición sea válida.";
            return RedirectToAction(nameof(Detalle), new { id });
        }
    }
}