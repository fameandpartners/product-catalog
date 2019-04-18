using Fame.Data.Models;
using Fame.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fame.Service.Integration
{
    [TestClass]
    public class CurationSearchTests : BaseTest
    {
        [TestMethod]
        public void DeleteCacheTest()
        {
            UnitOfWork.BeginTransaction();
            Services.Cache.Value.DeleteAll();
            UnitOfWork.Save();
            UnitOfWork.CommitTransaction();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task CurationAreNotNull()
        {
            var curations = await CurationSearchService.GetCurationsAsync(TestData.Curations, FameConfig.Localisation.AU, false);
            Assert.IsFalse(curations == null);
        }

        [TestMethod]
        public async Task CurationsAreAListOfProductListItem()
        {
            var curations = await CurationSearchService.GetCurationsAsync(TestData.Curations, FameConfig.Localisation.AU, false);
            Assert.IsInstanceOfType(curations, typeof(List<ProductListItem>));
        }

        [TestMethod]
        public async Task CurationAAndBAreReturned()
        {
            var curations = await CurationSearchService.GetCurationsAsync(TestData.Curations, FameConfig.Localisation.AU, false);
            var curationA = curations.FirstOrDefault(c => c.PID == TestData.CurationA);
            var curationB = curations.FirstOrDefault(c => c.PID == TestData.CurationB);
            Assert.IsFalse(curationA == null);
            Assert.IsFalse(curationB == null);
        }

        [TestMethod]
        public async Task CurationsHaveTheCorrectNumberOfMediaItems()
        {
            var curations = await CurationSearchService.GetCurationsAsync(TestData.Curations, FameConfig.Localisation.AU, false);
            var curationA = curations.FirstOrDefault(c => c.PID == TestData.CurationA);
            var curationB = curations.FirstOrDefault(c => c.PID == TestData.CurationB);
            Assert.IsTrue(curationA.Media.Count == 2);
            Assert.IsTrue(curationB.Media.Count == 1);
        }
    }
}
