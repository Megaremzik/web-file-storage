using Microsoft.Extensions.DependencyInjection;
using WS.Business.Services;
using System;


namespace WS.Business.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddSharing(this IServiceCollection services)
        {
            services.AddTransient<SharingService>();
        }
    }
}
