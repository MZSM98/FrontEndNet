using frontendnet.Models;
using System.Text.Json.Serialization;

namespace frontendnet.Services;

// 1. Creamos una clase temporal que coincida exactamente con la estructura cruda de Node.js
public class ProductoBackendResponse
{
    [JsonPropertyName("producto")]
    public Producto? Producto { get; set; }

    [JsonPropertyName("categoria")]
    public Categoria? Categoria { get; set; }
}

public class ProductosClientService(HttpClient client)
{
    public async Task<List<Producto>?> GetAsync(string? search)
    {
        // 2. Descargamos el JSON crudo. Cambiamos ?s= a ?titulo= porque eso espera Node.js
        var rawData = await client.GetFromJsonAsync<List<ProductoBackendResponse>>($"api/productos?titulo={search}");
        
        if (rawData == null) return new List<Producto>();

        // 3. Agrupamos los productos y sus categorías en un Diccionario
        var productosDict = new Dictionary<int, Producto>();

        foreach (var row in rawData)
        {
            if (row.Producto == null || row.Producto.ProductoId == null) continue;

            int pId = row.Producto.ProductoId.Value;

            // Si es la primera vez que vemos este producto, lo agregamos al diccionario
            if (!productosDict.ContainsKey(pId))
            {
                row.Producto.Categorias = new List<Categoria>();
                productosDict[pId] = row.Producto;
            }

            // Si la fila trae una categoría adjunta, se la agregamos a la lista de ese producto
            if (row.Categoria != null && row.Categoria.CategoriaId != null)
            {
                productosDict[pId].Categorias!.Add(row.Categoria);
            }
        }

        // Devolvemos la lista ya limpia y formateada
        return productosDict.Values.ToList();
    }

    public async Task<Producto?> GetAsync(int id)
    {
        // El endpoint api/productos/{id} original del backend devuelve 404 si no tiene categoría
        // y oculta categorías si tiene más de 1. 
        // Solución Frontend: Usamos nuestra función limpia de arriba y filtramos localmente.
        var todosLosProductos = await GetAsync("");
        return todosLosProductos?.FirstOrDefault(p => p.ProductoId == id);
    }

    public async Task PostAsync(Producto producto)
    {
        var response = await client.PostAsJsonAsync($"api/productos", producto);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(Producto producto)
    {
        var response = await client.PutAsJsonAsync($"api/productos/{producto.ProductoId}", producto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await client.DeleteAsync($"api/productos/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task PostAsync(int id, int categoriaid)
    {
        var response = await client.PostAsJsonAsync($"api/productos/{id}/categoria", new { categoriaid });
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id, int categoriaid)
    {
        var response = await client.DeleteAsync($"api/productos/{id}/categoria/{categoriaid}");
        response.EnsureSuccessStatusCode();
    }
}