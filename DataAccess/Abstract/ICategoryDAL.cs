using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;

namespace DataAccess.Abstract
{
    public interface ICategoryDAL:IRepositoryBase<Category>
    {
        void CreateCategory(List<AddCategoryDTO> models);
        Task UpdateCategoryAsync(Guid id ,List<UpdateCategoryDTO> models);
        Task UpdateCategoryByLangAsync(Guid id,string LangCode,UpdateCategoryDTO models);
        void DeleteCategory(Guid id);
        GetCategoryDTO GetCategoryByLang(Guid id,string LangCode);
        List<GetCategoryDTO> GetAllLanguages(Guid id);
    }
}
    