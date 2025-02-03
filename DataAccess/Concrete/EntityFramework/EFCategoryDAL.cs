using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;

namespace DataAccess.Concrete.EntityFramework
{

    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        public async void CreateCategory(List<AddCategoryDTO> models)
        {
            using var context = new AppDbContext();
            Category category = new Category();
            category.CreatedDate = DateTime.Now;
            context.Categories.Add(category);
            context.SaveChanges();
            for (int i = 0; i < models.Count; i++)
            {
                CategoryLanguage categoryLanguage = new()
                {
                    CategoryName = models[i].Name,
                    LangCode = models[i].LangCode,
                    CreatedDate = DateTime.Now,
                    CategoryId = category.Id
                };
                context.CategoryLanguages.Add(categoryLanguage);
            }
            context.SaveChanges();
        }

        public void DeleteCategory(Guid id)
        {
            using var context= new AppDbContext();
            var findCategory=context.Categories.FirstOrDefault(x=> x.Id == id);
            if (findCategory != null)
            {
                var findLanguages = context.CategoryLanguages.Where(y => y.CategoryId == id);
                context.CategoryLanguages.RemoveRange(findLanguages);
                context.Categories.Remove(findCategory);
                context.SaveChanges();
            }

        }

        public List<GetCategoryDTO> GetAllLanguages(Guid id)
        {
         using var context=new AppDbContext();
            var findCategories=context.CategoryLanguages.Where(x=>x.CategoryId == id).ToList();
            List<GetCategoryDTO> categoriesDTO = new List<GetCategoryDTO>();
            for (int i = 0; i < findCategories.Count; i++)
            {
                GetCategoryDTO getCategoryDTO = new GetCategoryDTO
                {
                    Name = findCategories[i].CategoryName,
                    LangCode = findCategories[i].LangCode,
                    CategoryId = findCategories[i].CategoryId
                };
                categoriesDTO.Add(getCategoryDTO);
            }
          
            return categoriesDTO;
        }

        public GetCategoryDTO GetCategoryByLang(Guid id, string LangCode)
        {
           using var context=new AppDbContext();
            var category=context.CategoryLanguages.FirstOrDefault(a=>a.CategoryId == id && a.LangCode==LangCode);
           
                GetCategoryDTO getCategoryDTO = new()
                {
                    Name = category.CategoryName,
                    LangCode = LangCode,
                    CategoryId = category.CategoryId
                };
                return getCategoryDTO;
            

        }

        public async Task UpdateCategoryAsync(Guid id, List<UpdateCategoryDTO> models)
        {
            using var context = new AppDbContext();
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            var categoryLanguage = context.CategoryLanguages.Where(x => x.CategoryId == id).ToList();

            context.CategoryLanguages.RemoveRange(categoryLanguage);
            for (int i = 0; i < models.Count; i++)
            {
                CategoryLanguage newCategoryLanguage = new()
                {
                    CategoryName = models[i].Name,
                    LangCode = models[i].LangCode,
                    CreatedDate =DateTime.Now,
                    UpdatedDate =DateTime.Now,
                    CategoryId = id
                };
                await context.CategoryLanguages.AddAsync(newCategoryLanguage);  
            }
            await context.SaveChangesAsync();
        }

        public async Task UpdateCategoryByLangAsync(Guid id, string LangCode, UpdateCategoryDTO models)
        {
            using var context = new AppDbContext();
            var category = context.Categories.FirstOrDefault(x => x.Id == id);
            var categoryLanguage = context.CategoryLanguages.Where(x => x.CategoryId == id).ToList();
            var findLanguage = categoryLanguage.FirstOrDefault(x => x.LangCode == LangCode);

            if (findLanguage != null)
            {
                findLanguage.CategoryName = models.Name;
                findLanguage.UpdatedDate = DateTime.Now;
                findLanguage.LangCode = models.LangCode;
              
            }


            await context.SaveChangesAsync();
        }
    }
}

       
    
    

