using Core.Utilities.Results.Abstract;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        Task<IResult> CreateAsync(AddProductDTO model);
        Task<IResult> UpdateAsync(Guid id,UpdateProductDTO model);
        IDataResult<GetProductDTO> GetById(Guid id,string LangCode);
    }
}
