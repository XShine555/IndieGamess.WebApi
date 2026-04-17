using System.ComponentModel.DataAnnotations;

namespace WebApi.Payment
{
    public class PaymentConfiguration
    {
        public const string SectionName = "Payment";

        [Required]
        public required string ApiKey { get; set; }

        [Required]
        public required string WebhookSecret { get; set; }
    }
}
