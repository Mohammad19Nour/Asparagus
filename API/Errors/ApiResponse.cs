namespace AsparagusN.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "Success",
            400 => "A bad request, you have made",
            401 => "Authorized, you are not",
            403 => "Forbidden",
            404 => "Resource, it was not",
            405 => "Methode type, it was wrong",
            500 => "Internal Server Error",
            _ => "Default error message"

        };
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
}