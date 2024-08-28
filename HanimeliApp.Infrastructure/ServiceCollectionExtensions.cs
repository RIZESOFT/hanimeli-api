using System.Reflection;
using HanimeliApp.Domain.Entities.Abstract;
using HanimeliApp.Domain.Repositories;
using HanimeliApp.Domain.UnitOfWorks;
using HanimeliApp.Infrastructure.Contexts;
using HanimeliApp.Infrastructure.Repositories;
using HanimeliApp.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HanimeliApp.Infrastructure
{
	public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string mainConnectionStr)
        {
            if (string.IsNullOrEmpty(mainConnectionStr))
                throw new ArgumentNullException(nameof(mainConnectionStr));

            var domainAssembly = Assembly.GetAssembly(typeof(IEntity))!;

            var mainEntities = domainAssembly.GetTypes().Where(t => t.Namespace!.Equals("HanimeliApp.Domain.Entities")).ToList();

            foreach (var mainEntity in mainEntities)
            {
                //var tPrimaryKey = mainEntity.GetInterfaces().FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEntity<>)).GenericTypeArguments[0];
                var irepoType = typeof(IGenericRepository<>);
                var repoType = typeof(GenericRepository<>);

                irepoType.MakeGenericType(mainEntity);
                repoType.MakeGenericType(mainEntity);

                services.AddScoped(irepoType, repoType);
            }

            //services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddDbContext<HanimeliDbContext>(options =>
                {
                    //options.UseMySQL(mainConnectionStr);
                    options.UseNpgsql(mainConnectionStr);
                    options.EnableDetailedErrors().EnableSensitiveDataLogging();
                });


            return services;
        }
    }
}
