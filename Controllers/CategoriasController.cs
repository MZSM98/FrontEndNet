using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class CategoriasController(CategoriasClientService categorias) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Categoria>? lista = [];
        try
        {
            lista = await categorias.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception) { } // Carga la lista vacía si hay error
        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        Categoria? item = null;
        try
        {
            item = await categorias.GetAsync(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception) { return NotFound(); }
        return View(item);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearAsync(Categoria itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await categorias.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No ha sido posible crear la categoría. Verifique los datos.");
            }
        }
        return View(itemToCreate);
    }

    public async Task<IActionResult> EditarAsync(int id)
    {
        Categoria? itemToEdit = null;
        try
        {
            itemToEdit = await categorias.GetAsync(id);
            if (itemToEdit == null) return NotFound();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception) { return RedirectToAction(nameof(Index)); }
        return View(itemToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> EditarAsync(int id, Categoria itemToEdit)
    {
        if (id != itemToEdit.CategoriaId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await categorias.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No ha sido posible actualizar la categoría. Inténtelo nuevamente.");
            }
        }
        return View(itemToEdit);
    }

    public async Task<IActionResult> Eliminar(int id, bool? showError = false)
    {
        Categoria? itemToDelete = null;
        try
        {
            itemToDelete = await categorias.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            // Aquí mostramos el error si el POST falló
            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No se puede eliminar esta categoría porque tiene productos asociados o está en uso.";
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception) { return RedirectToAction(nameof(Index)); }
        return View(itemToDelete);
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await categorias.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            // Atrapamos el error de base de datos y lo mandamos de regreso a la pantalla con la alerta
            return RedirectToAction(nameof(Eliminar), new { id, showError = true });
        }
    }
}