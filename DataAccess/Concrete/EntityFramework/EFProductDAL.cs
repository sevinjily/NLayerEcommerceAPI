using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public async Task CreateProductAsync(AddProductDTO model)
        {
            await using var context = new AppDbContext();
            Product product = new()
            {
                Discount=model.Discount,
                IsStock = model.IsStock,
                Price = model.Price,
                Review = model.Review,

            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            for (int i = 0; i < model.AddProductLanguageDTOs.Count; i++)
            {
                ProductLanguage productLanguage = new()
                {
                    ProductId = product.Id,
                    ProductName = model.AddProductLanguageDTOs[i].ProductName,
                    Description = model.AddProductLanguageDTOs[i].Description,
                    LangCode = model.AddProductLanguageDTOs[i].LangCode
                };
                await context.AddAsync(productLanguage);
            }
            await context.SaveChangesAsync();
            for (int i = 0; i < model.SubCategoryId.Count; i++)
            {
                ProductSubCategory productSubCategory = new()
                {
                    ProductId = product.Id,
                    SubCategoryId = model.SubCategoryId[i]
                };
                await context.AddAsync(productSubCategory);

            }
            await context.SaveChangesAsync();
        }
    }
}
