using System.Net;

namespace FormulaOne.Api.Models.Dtos;

public class HandlerResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class HandlerResult<T> : HandlerResult
{
    public T Data { get; set; } = default!;
}