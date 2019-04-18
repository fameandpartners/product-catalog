using Autofac;
using Fame.Search.Services;

namespace Fame.Search
{
    public class SearchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ElasticSearch>().As<IElasticSearch>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}
