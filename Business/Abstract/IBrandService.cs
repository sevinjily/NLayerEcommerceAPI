using Core.Utilities.Results.Abstract;
using Entities.DTOs.BrandDTOs;

namespace Business.Abstract
{
    
       public interface IBrandService
    {
        IResult Create(AddBrandDTO model);
        IDataResult<GetBrandDTO> GetBrands(Guid id);
        IResult Update(Guid id,UpdateBrandDTO model);
        IResult SoftDelete(Guid id);
        IResult HardDelete(Guid id);
    }
}
