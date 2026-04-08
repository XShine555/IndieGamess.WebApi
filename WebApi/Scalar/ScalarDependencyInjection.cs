using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace WebApi.Scalar
{
    public static class ScalarDependencyInjection
    {
        public static IServiceCollection ConfigureScalar(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IConfigureOptions<ScalarOptions>, ScalarOptionsSetup>();
            return serviceDescriptors;
        }
    }
}