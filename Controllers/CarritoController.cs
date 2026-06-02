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
        // 1. CAPA DE SEGURIDAD FRONTEND: Evitar manipulación por proxies o PTK
        // Rechazamos cantidades ilógicas o negativas inmediatamente
        if (cantidad <= 0 || cantidad > 10)
        {
            // Opcional: Podrías registrar esto en tu Bitácora como un intento de manipulación
            return RedirectToAction("Detalle", "Comprar", new { id = productoId });
        }

        try
        {
            var carrito = await carritoService.GetAsync();
            if (carrito == null)
            {
                carrito = await carritoService.CrearAsync();
            }

            var productoExistente = carrito?.Productos?.FirstOrDefault(p => p.Producto?.ProductoId == productoId);
            if (productoExistente != null)
            {
                cantidad += productoExistente.Cantidad;

                // 2. SEGUNDA CAPA: Validar que la suma total no exceda un límite por cliente
                if (cantidad > 10) cantidad = 10;
            }

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


    [HttpPost]
    public async Task<IActionResult> Eliminar(int productoId)
    {
        try
        {
            var carrito = await carritoService.GetAsync();
            if (carrito != null)
            {
                await carritoService.EliminarProductoAsync(carrito.Id, productoId);
            }
            return RedirectToAction("Index");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Pagar(int carritoId)
    {
        try
        {
            // Ejecutamos la compra en el backend
            await carritoService.PagarAsync(carritoId);
            
            // Si todo sale bien, lo mandamos a su lista de pedidos
            return RedirectToAction("Index", "Home");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch
        {
            // Opcional: si falla algo en la red, lo regresamos al carrito
            return RedirectToAction("Index");
        }
    }
}