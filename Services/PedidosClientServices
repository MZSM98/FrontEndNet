using frontendnet.Models;

namespace frontendnet.Services;

public class PedidosClientService(HttpClient client)
{
    // GET: /api/pedidos (Solo Admin)
    public async Task<List<Pedido>?> GetAsync()
    {
        return await client.GetFromJsonAsync<List<Pedido>>("api/pedidos");
    }

    // GET: /api/pedidos/clientes/{id} (Cliente)
    public async Task<List<Pedido>?> GetClienteAsync(string id)
    {
        return await client.GetFromJsonAsync<List<Pedido>>($"api/pedidos/clientes/{id}");
    }

    // GET: /api/pedidos/{id}/detalles (Admin)
    public async Task<DetallePedido?> GetDetalleAsync(int id)
    {
        return await client.GetFromJsonAsync<DetallePedido>($"api/pedidos/{id}/detalles");
    }

    // GET: /api/pedidos/clientes/{id}/detalles (Cliente)
    public async Task<DetallePedido?> GetDetalleClienteAsync(int id)
    {
        return await client.GetFromJsonAsync<DetallePedido>($"api/pedidos/clientes/{id}/detalles");
    }

    // PATCH: /api/pedidos/{id}/estado (Solo Admin)
    public async Task UpdateEstadoAsync(int id, string estado)
    {
        var response = await client.PatchAsJsonAsync($"api/pedidos/{id}/estado", new UpdateEstadoPedido { Estado = estado });
        response.EnsureSuccessStatusCode();
    }
}