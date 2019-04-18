using Autofac;
using Fame.Data.Models;
using Fame.Search.Services;
using Fame.Service.ChangeTracking;
using Fame.Service.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Service.Integration
{
    internal class TestData
    {
        internal static string CurationA = "FPG1001~1004~102~AK~B1~C7~T22~T76~T90~WB1";
        internal static string CurationB = "FPG1002~1009~102~B5~C10~KN~T0~T15~T79";

        internal static string[] Curations = 
        { 
            CurationA, 
            CurationB
        };

        internal static Dictionary<string, List<string>> CurationImagePaths = new Dictionary<string, List<string>> 
        {
            {
                CurationA,
                new List<string>() 
                    { 
                       "FPG1001~1004~102~AK~B1~C7~T22~T76~T90~WB1 1.jpg", 
                       "FPG1001~1004~102~AK~B1~C7~T22~T76~T90~WB1 2.jpg"
                    } 
            },
            {
                CurationB,
                new List<string>() 
                    {
                        "FPG1002~1009~102~B5~C10~KN~T0~T15~T79 1.jpg" 
                    }
            }
        };

        internal async static Task Initialise(IContainer container, DirectoryInfo fameIntegrationDirectory)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var importService = scope.Resolve<IImportService>();
                var cacheService = scope.Resolve<ICacheService>();
                var indexService = scope.Resolve<IIndexService>();
                var unitOfWork = scope.Resolve<IUnitOfWork>();
                var curationSearchService = scope.Resolve<ICurationSearchService>();
                var curationService = scope.Resolve<ICurationService>();
                var _curationMediaService = scope.Resolve<ICurationMediaService>();
                var curationImageCacheService = scope.Resolve<CurationImageCacheService>();

                // import from Product Catalogue (Integration)
                importService.Import();
                
                // index search
                await indexService.FullIndex();

                // Delete Test Curations
                foreach (var curationPid in Curations)
                {
                    curationService.Delete(curationPid);
                }
                unitOfWork.Save();

                // clear cache
                cacheService.DeleteAll();
                unitOfWork.Save();
                
                // add a curation
                var curations = new List<Curation>();
                foreach (var curationPid in Curations)
                {
                    curations.Add(await curationSearchService.UpsertCuration(curationPid));
                }
                unitOfWork.Save();
                
                // add a curated image
                foreach (var curation in curations)
                {
                    var files = CurationImagePaths[curation.PID].Select(fileName => new FileInfo($@"{fameIntegrationDirectory.FullName}\Images\{fileName}").AsMockIFormFile()).ToList();
                    var curationMedia = await _curationMediaService.AddMediaFormFiles(curation.PID, files);
                }
            }
        }
    }
}
