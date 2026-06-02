using System.Text.Json.Serialization;

namespace frontendnet.Services;

public class TiempoRespuesta
{
    // Fuerza a C# a buscar la propiedad "tiempo" en minúsculas en el JSON recibido
    [JsonPropertyName("tiempo")]
    public string Tiempo { get; set; } = string.Empty;
}

public class PerfilClientService(HttpClient client)
{
    public async Task<string> ObtenTiempoAsync()
    {
        try 
        {
            var respuesta = await client.GetFromJsonAsync<TiempoRespuesta>("api/auth/tiempo");
            return respuesta?.Tiempo ?? "00:00";
        }
        catch (Exception ex)
        {
            // Imprimirá el error real en tu consola (terminal) para facilitar el debug
            Console.WriteLine($"Error al obtener el tiempo: {ex.Message}");
            return "No disponible";
        }
    }
}