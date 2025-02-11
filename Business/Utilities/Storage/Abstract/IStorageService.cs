using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.Storage.Abstract
{
    public interface IStorageService :IStorage
    { 
        public string StorageName { get; }
    }
}
