using System;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.SQSEvents;
using Amazon.S3;
using Amazon.SQS;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fame.Common;
using Fame.Data;
using Fame.Data.Extensions;
using Fame.ImageGenerator.Workers;
using Fame.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fame.ImageGenerator
{
    public class LambdaEntryPoint
    {
        private readonly IContainer _applicationContainer;

        public LambdaEntryPoint()
        {
            // create service collection
            var services = new ServiceCollection();
            ConfigureServices(services);

            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);

            // Register all Autofac Modules in Assembly
            builder.RegisterModule<DataModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<ImageGeneratorModule>();

            /* builder.RegisterAssemblyModules();
             * foreach (var assembly in Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load))
            {
                builder.RegisterAssemblyModules(assembly);
            } */

            _applicationContainer = builder.Build();
        }


        public Task<Object> InvokeLayeringWorker(SQSEvent snsEvent)
        {
            return InvokeWorker<LayeringWorker, SQSEvent>(snsEvent);
        }

        public Task<Object> InvokeFileSyncWorker(SQSEvent snsEvent)
        {
            return InvokeWorker<FileSyncWorker, SQSEvent>(snsEvent);
        }

        public Task<Object> InvokeLayeringAdhocWorker(APIGatewayProxyRequest e)
        {
            return InvokeWorker<LayeringAdhocWorker, APIGatewayProxyRequest>(e);
        }

        private Task<Object> InvokeWorker<Worker, E>(E e) where Worker : IWorker<E>
        {
            using (var scope = _applicationContainer.BeginLifetimeScope())
            {
                var worker = scope.Resolve<Worker>();
                return worker.Process(e);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAutofac();

            // add logging
            services.AddSingleton(new LoggerFactory().AddConsole());
            services.AddLogging();

            // add config
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            services.AddSingleton<IConfiguration>(config);
            services.Configure<FameConfig>(config.GetSection(typeof(FameConfig).Name));

            services.AddDistributedCache(config);

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSQS>();
            services.AddAWSService<IAmazonLambda>();

        }
    }
}
