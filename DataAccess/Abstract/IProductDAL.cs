﻿using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;

namespace DataAccess.Abstract
{
    public interface IProductDAL :IRepositoryBase<Product>
    {
        Task CreateProductAsync(AddProductDTO model);
        Task UpdateProductAsync(Guid id, UpdateProductDTO model);
        GetProductDTO GetProductAsync(Guid id,string LangCode);
        Task DeleteProductAsync(Guid id);
    }
}
