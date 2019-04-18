using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;

namespace Fame.Service.Integration
{
    static class IFormFileExtensions
    {
        public static IFormFile AsMockIFormFile(this FileInfo physicalFile)
        {
            var fileMock = new Mock<IFormFile>();
            var physicalFilePath = physicalFile.FullName;
            var fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(physicalFile.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(() => physicalFile.OpenRead());
            fileMock.Setup(f => f.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            
            return fileMock.Object;
        }
    }
}
