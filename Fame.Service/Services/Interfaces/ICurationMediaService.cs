using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fame.Data.Models;
using Fame.Service.DTO;
using Microsoft.AspNetCore.Http;

namespace Fame.Service.Services
{
    public interface ICurationMediaService : IBaseService<CurationMedia>
    {
        Task<List<CurationMedia>> AddMediaFormFiles(string pid, List<IFormFile> files);
        Task<CurationMedia> AddMediaAtPosition(string pid, int sortOrder, string fileName, Stream stream, DateTime LastModified);
        List<PIDModel> AssignMediaListItems(List<PIDModel> pidModels, bool noMediaForCadImages);
        void Update(string pid, List<CurationMedia> curationMedia);
		string DeleteMedia(int id);
        bool ShouldAddMedia(string pid, int sortPosition, DateTime lastModified);
        void ArchiveAll();
        void DeleteArchivedMedia();
    }
}