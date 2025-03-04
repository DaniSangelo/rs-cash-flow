namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> ErrorMessage { get; set; } = [];

    public ResponseErrorJson(List<string> errorMessages)
    {
        ErrorMessage = errorMessages;
    }

    public ResponseErrorJson(string errorMessage)
    {
        ErrorMessage = [errorMessage];
    }
}
