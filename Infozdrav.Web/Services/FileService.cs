using System;
using System.IO;
using Infozdrav.Web.Abstractions;
using Infozdrav.Web.Data;
using Infozdrav.Web.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infozdrav.Web.Services
{
    public class FileService : IDependency
    {
        private readonly IOptions<FileSettings> _options;
        private readonly IHostingEnvironment _environment;
        private readonly AppDbContext _dbContext;

        public FileService(IOptions<FileSettings> options, IHostingEnvironment environment, AppDbContext dbContext)
        {
            _options = options;
            _environment = environment;
            _dbContext = dbContext;

            if (!Directory.Exists(GetFullPath()))
                Directory.CreateDirectory(GetFullPath());
        }


        public DataFile ShraniFile(IFormFile file)
        {
            if (file == null)
                return null;

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}.{ext}";
            var filePath = Path.Combine(GetFullPath(), fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
                file.CopyTo(fileStream);

            var fileTable = _dbContext.Set<DataFile>();
            var dataFile = new DataFile
            {
                UploadName = file.FileName,
                DiskName = fileName
            };
            fileTable.Add(dataFile);
            _dbContext.SaveChanges();

            return dataFile;
        } 

        public void DeleteFile(DataFile file)
        {
            if (file == null)
                return;

            var fileTable = _dbContext.Set<DataFile>();
            fileTable.Remove(file);

            if (FileExists(file))
                File.Delete(Path.Combine(GetFullPath(), file.DiskName));

            _dbContext.SaveChanges();
        }

        public Stream GetFile(DataFile file)
        {
            if (FileExists(file))
                return File.OpenRead(Path.Combine(GetFullPath(), file.DiskName));

            return null;
        }

        private bool FileExists(DataFile file)
        {
            return File.Exists(Path.Combine(GetFullPath(), file.DiskName));
        }

        private string GetFullPath()
        {
            return Path.Combine(_environment.WebRootPath, _options.Value.UploadDir);
        }
    }
}