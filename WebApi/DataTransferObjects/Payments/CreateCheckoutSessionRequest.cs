namespace WebApi.DataTransferObjects.Payments
{
    public record CreateCheckoutSessionRequest(
        string SuccessUrl,
        string CancelUrl);
}
