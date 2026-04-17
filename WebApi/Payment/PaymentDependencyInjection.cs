using Microsoft.Extensions.Options;
using Stripe;

namespace WebApi.Payment
{
    public static class PaymentDependencyInjection
    {
        public static IServiceCollection AddPayment(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddOptionsWithValidateOnStart<PaymentConfiguration>()
               .Bind(configuration.GetRequiredSection(PaymentConfiguration.SectionName))
               .ValidateDataAnnotations();

            serviceDescriptors.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<PaymentConfiguration>>().Value);

            serviceDescriptors.AddSingleton<IStripeClient>(serviceProvider =>
            {
                var paymentConfiguration = serviceProvider.GetRequiredService<PaymentConfiguration>();
                StripeConfiguration.ApiKey = paymentConfiguration.ApiKey;
                return new StripeClient(paymentConfiguration.ApiKey);
            });

            return serviceDescriptors;
        }
    }
}
