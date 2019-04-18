using Autofac;
using Autofac.Extras.AggregateService;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;

namespace Fame.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterAggregateService<IBaseServices>();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Client"))
                .AsImplementedInterfaces();
            
            builder.RegisterType<S3ProductRenderService>();
            builder.RegisterType<S3CurationImageService>();
            builder.RegisterType<S3DocumentService>();
            builder.RegisterType<LocalFileService>();

            builder.RegisterType<ProductRenderCacheService>();
            builder.RegisterType<CurationImageCacheService>();
        }
    }
}
