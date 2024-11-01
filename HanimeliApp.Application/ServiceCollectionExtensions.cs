using System.Reflection;
using Azure.Storage.Blobs;
using HanimeliApp.Application.Services;
using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Application.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HanimeliApp.Application
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var domainServices = Assembly.GetAssembly(typeof(ServiceBase<,,,>))!
                .GetTypes()
                .Where(t => !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && 
                            t.BaseType.GetGenericTypeDefinition() == typeof(ServiceBase<,,,>))
                .ToList();
            domainServices.AddRange(Assembly.GetAssembly(typeof(ServiceBase))!.GetTypes().Where(t => t.IsSubclassOf(typeof(ServiceBase))).ToList());
            var azureStorageActive = configuration.GetValue<bool>("AzureStorageConfig:Enabled");
            if (azureStorageActive)
            {
                services.AddScoped<ImageService>();
                services.AddSingleton(x => new AzureStorageHelper(new BlobServiceClient(configuration["AzureStorageConfig:ConnectionString"])));
            }

            foreach (var service in domainServices)
            {
                services.AddScoped(service);
            }
            
            return services;
        }
    }
}
