using frontendnet.Models;

namespace frontendnet.Services;

public class CarritoClientService(HttpClient client)
{
    public async Task<Carrito?> GetAsync()
    {
        try
        {
            return await client.GetFromJsonAsync<Carrito>("api/carritos");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Si Node.js devuelve 404, significa que no hay carrito abierto aún
            return null; 
        }
    }

    public async Task<Carrito?> CrearAsync()
    {
        var response = await client.PostAsync("api/carritos", null);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Carrito>();
    }

    public async Task AgregarProductoAsync(int carritoId, int productoId, int cantidad)
    {
        // En Node.js el body espera { cantidad: X }
        var response = await client.PutAsJsonAsync($"api/carritos/{carritoId}/producto/{productoId}", new { cantidad });
        response.EnsureSuccessStatusCode();
    }

    public async Task EliminarProductoAsync(int carritoId, int productoId)
    {
        // Llama a la ruta DELETE del backend de tu compañero
        var response = await client.DeleteAsync($"api/carritos/{carritoId}/producto/{productoId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task PagarAsync(int carritoId)
    {
        // Llama a la ruta de checkout de Node.js
        var response = await client.PostAsync($"api/carritos/{carritoId}/checkout", null);
        response.EnsureSuccessStatusCode();
    }
}