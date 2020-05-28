using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using YodaIM.Models;

namespace YodaIM.Services.Database
{
    public class FileService : DatabaseServiceBase, IFileService
    {
        private static char[] TRIM_CHARS = new char[] { '"', ' ' };

        public FileService(Context context) : base(context)
        {
        }

        public Task<FileModel> Get(Guid id)
        {
            return context.FileModels
                .Where(f => f.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<ICollection<FileModel>> GetAll(ICollection<Guid> ids)
        {
            return await context.FileModels.Where(fm => ids.Contains(fm.Id)).ToListAsync();
        }

        public Task<List<FileModel>> GetUserFiles(User user, DateTime? after)
        {
            IQueryable<FileModel> files = context.FileModels;

            if (after != null)
                files = files.Where(fm => fm.UploadedAt < after);

            return files
                .Where(fm => fm.UserId == user.Id)
                .OrderBy(fm => fm.UploadedAt)
                .Take(50)
                .ToListAsync();
        }

        public Task<FileModel> GetWithData(Guid id)
        {
            return context.FileModels
                .Where(f => f.Id == id)
                .Include(f => f.BinaryBlob)
                .SingleOrDefaultAsync();
        }

        public async Task<FileModel> Upload(IFormFile file, User user, FileType fileType)
        {
            var fileName = file.FileName.Trim(TRIM_CHARS);
            string contentType = string.Empty;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            contentType = contentType ?? "application/octet-stream";

            var buffer = new byte[file.Length];

            await file.OpenReadStream().ReadAsync(buffer, 0, (int)file.Length);

            var fileModel = new FileModel
            {
                FileName = fileName,
                ContentType = contentType,
                UserId = user.Id,
                Type = fileType,
                Size = (int)file.Length,
                BinaryBlob = new BinaryBlob
                {
                    Data = buffer,
                    Sha256 = SHA256.Create().ComputeHash(buffer)
                }
            };
            context.FileModels.Add(fileModel);
            await context.SaveChangesAsync();
            return fileModel;
        }
    }
}
