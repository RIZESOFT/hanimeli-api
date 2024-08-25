using System.Reflection;
using HanimeliApp.Application.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace HanimeliApp.Application
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var domainServices = Assembly.GetAssembly(typeof(ServiceBase))!.GetTypes().Where(t => t.IsSubclassOf(typeof(ServiceBase))).ToList();

            foreach (var service in domainServices)
            {
                services.AddScoped(service);
            }
            
            return services;
        }
    }
}
