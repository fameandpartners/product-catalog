using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fame.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using Fame.Data.Extensions;
using Fame.Service.ChangeTracking;
using Fame.Web.Code.Extensions;
using Fame.Common;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.SQS;
using Amazon.Lambda;
using Fame.Search.Services;
using Microsoft.Extensions.Options;

namespace Fame.Service.Integration
{
    [TestClass]
    public class BaseTest
    {
        private static ILifetimeScope Scope { get; set; }

        private static IContainer Container { get; set; }
        protected FameConfig FameConfig { get; private set; }

        protected IBaseServices Services { get; set; }

        protected IUnitOfWork UnitOfWork { get; set; }

        protected ICurationSearchService CurationSearchService { get; private set; }

        [AssemblyInitialize()]
        public static async Task AssemblyInit(TestContext context)
        {
            var fameWebRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.GetDirectories().First(d => d.Name == "Fame.Web");

            var fameIntegrationDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($@"{fameWebRoot.FullName}\appsettings.json")
                .AddJsonFile($@"{fameWebRoot.FullName}\appsettings.Development.json")
                .AddJsonFile($@"{fameIntegrationDirectory.FullName}\appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
                       
            services.AddOptions();
            
            services.Configure<FameConfig>(configuration.GetSection(typeof(FameConfig).Name));

            services.AddDbContext(configuration);

            services.AddLogging();

            services.AddDistributedCache(configuration);
            
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            services.AddAWSService<IAmazonS3>();

            services.AddAWSService<IAmazonSQS>();

            services.AddAWSService<IAmazonLambda>();

            var builder = new ContainerBuilder();

            builder.Populate(services);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Fame")).ToArray();

            builder.RegisterAssemblyModules(assemblies);
                        
            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var importService = scope.Resolve<IImportService>();
                var indexService = scope.Resolve<IIndexService>();
            }

            await TestData.Initialise(Container, fameIntegrationDirectory);
        }

        [TestInitialize()]
        public void Initialize()
        {
            Scope = Container.BeginLifetimeScope();

            FameConfig = Scope.Resolve<IOptions<FameConfig>>().Value;
            Services = Scope.Resolve<IBaseServices>();
            UnitOfWork = Scope.Resolve<IUnitOfWork>();
            CurationSearchService = Scope.Resolve<ICurationSearchService>();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            CurationSearchService = null;
            Services = null;
            UnitOfWork = null;

            Scope.Dispose();
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup() 
        {
            Container.Dispose();
        }
    }
}
