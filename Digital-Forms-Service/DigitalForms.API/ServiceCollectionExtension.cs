

using DigitalForms.BL.Interfaces;
using DigitalForms.BL.Serialized.Models;
using DigitalForms.DL.Repositories;
using DigitalForms.DL.UnitOfWork;
using AutoMapper;
using DigitalForms.DL.Data;

namespace DigitalForms.API
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            //services.AddAutoMapper((serviceProvider, automapper) =>
            //{
            //    automapper.AddCollectionMappers();
            //    automapper.UseEntityFrameworkCoreModel<DFSqlContext>(serviceProvider);
            //}, typeof(DFSqlContext).Assembly);

            //var serviceProvider = services.BuildServiceProvider();
        }
        public static void AddConfigurations(this IServiceCollection services)
        {
            //services.AddSingleton<IStorageConfiguration, StorageConfiguration>();
            //services.AddSingleton<IServiceBusConfiguration, ServiceBusConfiguration>();
            
            
        }

      
        public static void AddInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
        {            
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient<DigitalForms.BL.Interfaces.ISerializer, DigitalForms.BL.Serialized.Models.Serializer>();
        }




    }
}

