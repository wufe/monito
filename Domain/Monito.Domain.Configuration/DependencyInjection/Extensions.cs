using Microsoft.Extensions.DependencyInjection;
using Monito.Domain.Service;
using Monito.Domain.Service.Interface;

namespace Monito.Domain.Configuration.DependencyInjection
{
    public static class Extensions {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<ILinkService, LinkService>();
            services
                .AddScoped<IRequestService, RequestService>();
            services
                .AddScoped<IUserService, UserService>();
        }
    }
}