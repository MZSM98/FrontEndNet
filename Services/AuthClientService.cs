using frontendnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace frontendnet.Services;

public class AuthClientService(HttpClient client, IHttpContextAccessor httpContextAccessor)
{
    public async Task<AuthUser> ObtenTokenAsync(string email, string password)
    {
        // === CÓDIGO REAL (COMENTADO TEMPORALMENTE) ===
        /*
        Login usuario = new() { Email = email, Password = password };
        var response = await client.PostAsJsonAsync("api/auth", usuario);
        var token = await response.Content.ReadFromJsonAsync<AuthUser>();
        return token!;
        */

        // === CÓDIGO MOCK PARA PRUEBAS SIN BACKEND ===
        string tokenFalsoAdmin = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3ZlcmFAdXYubXgiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJHdWlsbGVybW8gVmVyYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmFkb3IiLCJpc3MiOiJQUy1KV1QiLCJhdWQiOiJDbGllbnRlc1BTLUpXVCIsImlhdCI6MTc4MDE4OTg0NiwiZXhwIjoxNzgwMTkxMDQ2fQ.vEd8jC61f3cqgqdw9qjGDFvvTb8FIZkEoDNeAYXiqUs";

        // Devolvemos el AuthUser llenando TODAS sus propiedades 'required'
        return await Task.FromResult(new AuthUser 
        { 
            Email = email, // Usamos el que pongas en el formulario
            Nombre = "Administrador de Prueba", 
            Rol = "Administrador",
            Jwt = tokenFalsoAdmin 
        });
    }

    public async void IniciaSesionAsync(List<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();
        await httpContextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties)!;
    }
    
  
    public Task RegistrarClienteAsync(Registro modelo)
    {
        // === CÓDIGO REAL (COMENTADO HASTA QUE CONECTES TU BACKEND) ===
        /*
        var response = await client.PostAsJsonAsync("api/usuarios/clientes", modelo);
        response.EnsureSuccessStatusCode();
        */

        // === MOCK ESTÁTICO PARA PROBAR LA UI SIN BACKEND ===
        // Simula que la llamada a la API fue exitosa sin hacer la petición HTTP real
        return Task.CompletedTask;
    }
}