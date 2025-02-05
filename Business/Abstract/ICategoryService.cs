using Entities.DTOs.CategoryDTOs;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        void Create(List<AddCategoryDTO> models);
        Task Update(Guid id,List<UpdateCategoryDTO> models);
        Task UpdateByLang(Guid id,string lang,UpdateCategoryDTO model);
        void Delete(Guid id);
        GetCategoryDTO GetByLang(Guid id,string LangCode);
        List<GetCategoryDTO> GetAllLanguages(Guid id);
    }
}
