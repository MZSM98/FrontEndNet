using System.Security.Claims;
using frontendnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize]
public class PerfilController : Controller
{
    public IActionResult Index()
    {
        AuthUser usuario = new AuthUser
        {
            Email = User.FindFirstValue(ClaimTypes.Name) ?? "",
            Nombre = User.FindFirstValue(ClaimTypes.GivenName) ?? "",
            Rol = User.FindFirstValue(ClaimTypes.Role) ?? "",
            Jwt = User.FindFirstValue("jwt") ?? ""
        };

        return View(usuario);
    }
}