using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace WebApi.Authentication
{
    public static class AuthenticationDependencyInjection
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            serviceDescriptors.AddOptionsWithValidateOnStart<AuthenticationConfiguration>()
               .Bind(configuration.GetRequiredSection(AuthenticationConfiguration.SectionName))
               .ValidateDataAnnotations();

            serviceDescriptors.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<AuthenticationConfiguration>>().Value);

            serviceDescriptors.AddScoped<JwtBearerEventsHandler>();
            serviceDescriptors.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsSetup>();
            serviceDescriptors.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return serviceDescriptors;
        }
    }
}
