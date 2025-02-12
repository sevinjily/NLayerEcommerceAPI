using Business.Utilities.Storage.Abstract;
using Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.Storage.Concrete
{
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage; 
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task<Upload> UploadFileAsync(string path, IFormFile file)
        => await _storage.UploadFileAsync(path, file);
    }
}
