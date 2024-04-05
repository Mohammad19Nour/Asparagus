namespace AsparagusN.Errors;

public class ApiErrorResponse : ApiResponse
{
    public object Data { get; }
    public int StatusCode { get; set; }
    public ApiErrorResponse(object result) : base(400)
    {
        Data = result;
        StatusCode = 400;
    }
}