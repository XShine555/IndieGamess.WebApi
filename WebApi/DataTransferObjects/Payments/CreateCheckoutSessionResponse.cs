namespace WebApi.DataTransferObjects.Payments
{
    public record CreateCheckoutSessionResponse(
        string SessionId,
        string Url);
}
