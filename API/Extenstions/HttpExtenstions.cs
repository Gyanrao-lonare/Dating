using System.Text.Json;
using API.Helpers;

namespace API.Extenstions
{
    public static class HttpExtenstions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,
        int itemsPerPage, int totalItems, int totalPages)
        {

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}