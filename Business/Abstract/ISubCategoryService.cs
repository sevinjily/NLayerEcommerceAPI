using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.SubCategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISubCategoryService
    {
        IResult Create(AddSubCategoryDTO model);
        IResult Update(Guid id, UpdateSubCategoryDTO model);
        IResult Delete(Guid id);
        IDataResult<GetSubCategoryDTO> Get(Guid id);
    }
}
