
using Autofac;
using Autofac.Extras.AggregateService;
using Fame.Data.Repository;

namespace Fame.Data
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<IRepositories>();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }
}
