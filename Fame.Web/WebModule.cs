using Autofac;
using AutofacSerilogIntegration;

namespace Fame.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
        }
    }
}
