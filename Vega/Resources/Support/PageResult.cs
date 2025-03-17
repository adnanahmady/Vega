using System.Text.Json;

using Microsoft.AspNetCore.Mvc;

namespace Vega.Resources.Support;

public class PageResult<T> : IActionResult
{
    public required IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(new
        {
            Data = Items,
            Meta = new
            {
                Pagination = new
                {
                    CurrentPage,
                    PageSize,
                    TotalRecords,
                    From = PageSize * (CurrentPage > 0 ? CurrentPage - 1 : 0),
                    To = PageSize * CurrentPage
                }
            }
        }, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(json);
    }
}
