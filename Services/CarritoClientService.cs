using frontendnet.Models;

namespace frontendnet.Services;

public class CarritoClientService(HttpClient client)
{
    public async Task<Carrito?> GetAsync()
    {
        try
        {
            return await client.GetFromJsonAsync<Carrito>("api/carrito");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Si Node.js devuelve 404, significa que no hay carrito abierto aún
            return null; 
        }
    }

    public async Task<Carrito?> CrearAsync()
    {
        var response = await client.PostAsync("api/carrito", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Carrito>();
    }

    public async Task AgregarProductoAsync(int carritoId, int productoId, int cantidad)
    {
        // En Node.js el body espera { cantidad: X }
        var response = await client.PutAsJsonAsync($"api/carrito/{carritoId}/producto/{productoId}", new { cantidad });
        response.EnsureSuccessStatusCode();
    }
}