namespace MaoNaMassa.API.Helpers;

/// <summary>
/// Helper para criar links HATEOAS nas respostas da API
/// </summary>
public static class HateoasHelper
{
    /// <summary>
    /// Cria um objeto de link HATEOAS
    /// </summary>
    public static object CreateLink(string rel, string href, string method = "GET")
    {
        return new
        {
            rel = rel,
            href = href,
            method = method
        };
    }

    /// <summary>
    /// Cria links padrão para um recurso (self, update, delete)
    /// </summary>
    public static List<object> CreateResourceLinks(string baseUrl, Guid id, bool includeDelete = true)
    {
        var links = new List<object>
        {
            CreateLink("self", $"{baseUrl}/{id}", "GET"),
            CreateLink("update", $"{baseUrl}/{id}", "PUT")
        };

        if (includeDelete)
        {
            links.Add(CreateLink("delete", $"{baseUrl}/{id}", "DELETE"));
        }

        return links;
    }

    /// <summary>
    /// Cria links de paginação
    /// </summary>
    public static List<object> CreatePaginationLinks(
        string baseUrl,
        int paginaAtual,
        int totalPaginas,
        Dictionary<string, string>? queryParams = null)
    {
        var links = new List<object>();
        var queryString = BuildQueryString(queryParams);

        // Self
        links.Add(CreateLink("self", $"{baseUrl}?pagina={paginaAtual}{queryString}", "GET"));

        // First
        if (paginaAtual > 1)
        {
            links.Add(CreateLink("first", $"{baseUrl}?pagina=1{queryString}", "GET"));
        }

        // Previous
        if (paginaAtual > 1)
        {
            links.Add(CreateLink("prev", $"{baseUrl}?pagina={paginaAtual - 1}{queryString}", "GET"));
        }

        // Next
        if (paginaAtual < totalPaginas)
        {
            links.Add(CreateLink("next", $"{baseUrl}?pagina={paginaAtual + 1}{queryString}", "GET"));
        }

        // Last
        if (paginaAtual < totalPaginas)
        {
            links.Add(CreateLink("last", $"{baseUrl}?pagina={totalPaginas}{queryString}", "GET"));
        }

        return links;
    }

    private static string BuildQueryString(Dictionary<string, string>? queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
            return string.Empty;

        var pairs = queryParams
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Key != "pagina")
            .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}");

        var queryString = string.Join("&", pairs);
        return string.IsNullOrEmpty(queryString) ? string.Empty : $"&{queryString}";
    }
}

