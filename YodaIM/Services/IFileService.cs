using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services
{
    public interface IFileService
    {
        Task<FileModel> Upload(IFormFile file, User user, FileType fileType);

        Task<FileModel> Get(Guid id);

        Task<FileModel> GetWithData(Guid id);

        Task<ICollection<FileModel>> GetAll(ICollection<Guid> ids);
    }
}
