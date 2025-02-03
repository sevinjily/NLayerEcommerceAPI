using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        void Create(List<AddCategoryDTO> models);
        Task Update(Guid id,List<UpdateCategoryDTO> models);
        Task UpdateByLang(Guid id,string lang,UpdateCategoryDTO model);
        void Delete(Guid id);
        GetCategoryDTO GetByLang(Guid id,string LangCode);
        List<GetCategoryDTO> Get(Guid id);
    }
}
