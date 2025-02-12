using Business.Utilities.Storage.Abstract.LocalStorage;
using Entities.Common;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;


namespace Business.Utilities.Storage.Concrete.LocalStorage
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _env;

        public LocalStorage(IWebHostEnvironment env)
        {
            _env = env;
        }


        public async Task<Upload> UploadFileAsync(string path, IFormFile file)
        {
            string uploadPath = Path.Combine(_env.WebRootPath, path);
            if (!Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);

            var newFileName = Guid.NewGuid().ToString() + file.FileName;
            var pathCombine = Path.Combine(uploadPath, newFileName);

            using FileStream fileStream = new FileStream(pathCombine, FileMode.Create);

            await file.CopyToAsync(fileStream);
            return new Upload
            {
                FileName = newFileName,
                Path = uploadPath
            };
        }
      
    }
}
