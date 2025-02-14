using Core.Utilities.Results.Abstract;
using Entities.DTOs.BrandDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    
       public interface IBrandService
    {
        IResult Create(AddBrandDTO model);
        IDataResult<GetBrandDTO> GetBrands(Guid id);
        IResult Update(Guid id,UpdateBrandDTO model);
        IResult SoftDelete(Guid id);
    }
}
