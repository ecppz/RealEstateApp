using Domain.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<RealEstateAppContext>(opt =>
                                              opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContext<RealEstateAppContext>(
                  (serviceProvider, opt) =>
                    {
                        opt.EnableSensitiveDataLogging();
                        opt.UseSqlServer(connectionString,
                        m => m.MigrationsAssembly(typeof(RealEstateAppContext).Assembly.FullName));
                    },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped
                 );

                #endregion

                #region Repositories IOC
                services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                services.AddScoped<IFavoriteRepository, FavoriteRepository>();
                services.AddScoped<IPropertyRepository, PropertyRepository>();
                services.AddScoped<IImprovementRepository, ImprovementRepository>();
                services.AddScoped<IMessageRepository, MessageRepository>();
                services.AddScoped<IOfferRepository, OfferRepository>();
                services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
                services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
                services.AddScoped<IPropertyImprovementRepository, PropertyImprovementRepository>();
                #endregion
            }
        }
    }
}
