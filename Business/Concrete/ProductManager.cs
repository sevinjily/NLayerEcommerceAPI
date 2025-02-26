using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccesResults;
using DataAccess.Abstract;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDAL _productDAL;

        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public async Task<IResult> CreateAsync(AddProductDTO model)
        {
            await _productDAL.CreateProductAsync(model);
            return new SuccessResult(System.Net.HttpStatusCode.Created);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
           await _productDAL.DeleteProductAsync(id);
           return  new SuccessResult(System.Net.HttpStatusCode.OK);
        }

        public IDataResult<GetProductDTO> GetById(Guid id, string LangCode)
        {
           var data = _productDAL.GetProductAsync(id, LangCode);
            if (data == null)
            
                return new ErrorDataResult<GetProductDTO>(message:"Melumat sehvdir yeniden cehd edin",System.Net.HttpStatusCode.NotFound);
          
            if(data.IsStock==false)
                return new ErrorDataResult<GetProductDTO>(message: "Mehsul bitib", System.Net.HttpStatusCode.NotFound);

            return new SuccessDataResult<GetProductDTO>(data,System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<IResult> UpdateAsync(Guid id, UpdateProductDTO model)
        {
           await _productDAL.UpdateProductAsync(id, model);
            return new SuccessResult(System.Net.HttpStatusCode.OK);
        }
    }
}
