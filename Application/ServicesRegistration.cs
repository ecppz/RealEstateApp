using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServicesRegistration
    {
        //Extension method - Decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(opt=> opt.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            #endregion
            #region Services IOC
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IImprovementService, ImprovementService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISaleTypeService, SaleTypeService>();

            #endregion
        }
    }
}
