using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

[Authorize(Roles = "Administrador")]
public class UsuariosController(UsuariosClientService usuarios, RolesClientService roles) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Usuario>? lista = [];
        try
        {
            lista = await usuarios.GetAsync();
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            // Error genérico para la vista principal
            ViewBag.ErrorMessage = "Ha ocurrido un error al cargar la lista de usuarios. Inténtelo más tarde.";
        }
        return View(lista);
    }

    public async Task<IActionResult> Detalle(string id)
    {
        try
        {
            var item = await usuarios.GetAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "No se pudo cargar la información del usuario.";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Crear()
    {
        try
        {
            await RolesDropDownListAsync();
            return View();
        }
        catch (Exception)
        {
            
            TempData["ErrorMessage"] = "No es posible crear usuarios en este momento.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearAsync(UsuarioPwd itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            // NUEVO BLOQUE: Atrapamos el rechazo por correo duplicado (400 o 409)
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest || ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Asociamos el error específicamente al campo "Email"
                ModelState.AddModelError("Email", "Este correo ya se encuentra registrado en el sistema.");
            }
            catch (Exception )
            {
                // Error genérico por si se cae la red o el servidor
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al intentar guardar el usuario.");
    
            }
        }

        try { await RolesDropDownListAsync(); } catch { }
        return View(itemToCreate);
    }

    [HttpGet("[controller]/[action]/{email}")]
    public async Task<IActionResult> EditarAsync(string email)
    {
        try
        {
            var itemToEdit = await usuarios.GetAsync(email);
            if (itemToEdit == null) return NotFound();

            ViewBag.PuedeEditar = !(User.Identity?.Name == email);
            await RolesDropDownListAsync(itemToEdit?.Rol);
            return View(itemToEdit);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "No se pudo cargar la información para editar.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("[controller]/[action]/{email}")]
    public async Task<IActionResult> EditarAsync(string email, Usuario itemToEdit)
    {
        if (email != itemToEdit.Email) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.PutAsync(itemToEdit);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al intentar actualizar el usuario.");
            }
        }

        ViewBag.PuedeEditar = !(User.Identity?.Name == email);
        try { await RolesDropDownListAsync(itemToEdit?.Rol); } catch { }
        return View(itemToEdit);
    }

    public async Task<IActionResult> Eliminar(string id, bool? showError = false)
    {
        try
        {
            var itemToDelete = await usuarios.GetAsync(id);
            if (itemToDelete == null) return NotFound();

            if (showError.GetValueOrDefault())
                ViewData["ErrorMessage"] = "No ha sido posible eliminar el usuario. Es posible que tenga datos relacionados.";

            ViewBag.PuedeEditar = !(User.Identity?.Name == id);
            return View(itemToDelete);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Salir", "Auth");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "No se pudo cargar la información del usuario para eliminar.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(string id)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Auth");
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Eliminar), new { id, showError = true });
            }
        }
        return RedirectToAction(nameof(Eliminar), new { id, showError = true });
    }

    private async Task RolesDropDownListAsync(object? rolSeleccionado = null)
    {
        var listado = await roles.GetAsync();
        ViewBag.Rol = new SelectList(listado, "Nombre", "Nombre", rolSeleccionado);
    }
}