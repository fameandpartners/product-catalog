using System.Runtime.InteropServices;
using Amazon.Lambda.Core;
using Autofac;
using Fame.ImageGenerator.Services;
using Fame.ImageGenerator.Workers;
using Fame.Service;
using Fame.Service.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Fame.ImageGenerator
{
    public class ImageGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DropboxService>();
            builder.RegisterType<S3ProductRenderService>();
            builder.RegisterType<LocalFileService>();

            builder.RegisterType<FileSyncMaster>();
            builder.RegisterType<FileSyncWorker>();
            builder.RegisterType<LayeringMaster>();
            builder.RegisterType<LayeringWorker>();
            builder.RegisterType<LayeringAdhocWorker>();
            
            builder.RegisterType<LayerCombinationService>().AsImplementedInterfaces();
            builder.RegisterType<ImageCacheService>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                builder.RegisterType<ImageSharpImageManipulatorService>().AsImplementedInterfaces();
            }
            else
            {
                builder.RegisterType<ImageSharpImageManipulatorService>().AsImplementedInterfaces();
                //builder.RegisterType<MagickDotNetImageManipulatorService>().AsImplementedInterfaces();
            }

        }
    }
}
