namespace WebApi.DataTransferObjects.Payments.Requests
{
    public record CreateCheckoutSessionRequest(
        string SuccessUrl,
        string CancelUrl);
}
