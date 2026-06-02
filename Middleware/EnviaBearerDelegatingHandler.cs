using System.Net.Http.Headers;
using System.Security.Claims;

namespace frontendnet.Middlewares;

public class EnviaBearerDelegatingHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var path = request.RequestUri?.AbsolutePath ?? "";
        
        // PARCHE: Si pedimos una imagen pura (ej. /api/archivos/1), no enviamos el token. 
        // Así Node.js la trata como una petición pública y no nos bloquea por ser "Clientes".
        if (request.Method == HttpMethod.Get && path.StartsWith("/api/archivos/") && !path.EndsWith("/detalle"))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            var token = httpContextAccessor.HttpContext.User.FindFirstValue("jwt");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}