using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccesResults;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs.SubCategoryDTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Business.Concrete
{
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly ISubCategoryDAL _subCategoryDAL;

        public SubCategoryManager(ISubCategoryDAL subCategoryDAL)
        {

            _subCategoryDAL = subCategoryDAL;
        }

        public IResult Create(AddSubCategoryDTO model)
        {
            try
            {

                _subCategoryDAL.Add(new()
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                }
                    );
                return new SuccessResult("Uğurla əlavə olundu!",statusCode:System.Net.HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message,System.Net.HttpStatusCode.BadRequest);
            }
        }

        public IResult Delete(Guid id)
        {
            var context = new AppDbContext();
            var findSubCategory=context.SubCategories.FirstOrDefault(c=>c.Id == id);
            if (findSubCategory != null)
            {
                context.Remove(findSubCategory);
                context.SaveChanges();
            return new SuccessResult(HttpStatusCode.OK);
            }
            else
            {
                return new ErrorResult(HttpStatusCode.NotFound);
            }
        }

        public IDataResult<GetSubCategoryDTO> Get(Guid id)
        {
            var context=new AppDbContext();
            var findSubCategory=context.SubCategories.FirstOrDefault(b=>b.Id== id);
            if (findSubCategory != null)
            {
                GetSubCategoryDTO getSubCategoryDTO = new()
                {
                    Name = findSubCategory.Name,
                    CreatedDate = findSubCategory.CreatedDate,
                    UpdatedDate = findSubCategory.UpdatedDate.HasValue?findSubCategory.UpdatedDate.Value:default(DateTime)
                };
                return new SuccessDataResult<GetSubCategoryDTO>(data:getSubCategoryDTO,statusCode:HttpStatusCode.OK);
            }
            else
            {
                return new ErrorDataResult<GetSubCategoryDTO>(HttpStatusCode.NotFound);
            }
        }

        public IResult Update(Guid id, UpdateSubCategoryDTO model)
        {
          
                var context=new AppDbContext();
                var findSubCategory=context.SubCategories.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (findSubCategory != null)
                {

                findSubCategory.Name = model.Name;

                _subCategoryDAL.Update(findSubCategory);
                return new SuccessResult("Ugurla yerine yetirildi",HttpStatusCode.OK);
            }
            else
            {
                return new ErrorResult(HttpStatusCode.BadRequest);
            }

           
          
        }
    }
}
