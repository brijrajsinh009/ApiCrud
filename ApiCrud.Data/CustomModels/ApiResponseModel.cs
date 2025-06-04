namespace ApiCrud.Data.CustomModels;

public class ApiResponseModel
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public Object Data { get; set; }
}
