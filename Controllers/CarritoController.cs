using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Cliente,Usuario")]
public class CarritoController(CarritoClientService carritoService, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index()
    {
        Carrito? carrito = null;
        try
        {
            carrito = await carritoService.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        
        ViewBag.Url = configuration["UrlWebAPI"];
        return View(carrito);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(int productoId, int cantidad)
    {
        try
        {
            // 1. Buscamos si hay carrito
            var carrito = await carritoService.GetAsync();
            
            // 2. Si no hay, lo creamos
            if (carrito == null)
            {
                carrito = await carritoService.CrearAsync();
            }

            // 3. Agregamos el producto al ID del carrito
            await carritoService.AgregarProductoAsync(carrito!.Id, productoId, cantidad);
            
            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch
        {
            return RedirectToAction("Detalle", "Comprar", new { id = productoId });
        }
    }
}