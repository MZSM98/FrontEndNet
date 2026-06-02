using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class BitacoraController(BitacoraClientService bitacora) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Bitacora>? lista = [];
        try
        {
            lista = await bitacora.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            // Ahora nos dirá exactamente dónde se atoró
            ViewBag.ErrorMessage = "No se pudo cargar la bitácora: ";
        }
        return View(lista);
    }   
}