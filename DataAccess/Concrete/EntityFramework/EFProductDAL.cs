using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public async Task CreateProductAsync(AddProductDTO model)
        {
            await using var context = new AppDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();//bir nece defe saveasync etmemek ucun transaction yaziriq
            try
            {
                Product product = new()
                {
                    Discount = model.Discount,
                    IsStock = model.IsStock,
                    Price = model.Price,
                    Review = model.Review,

                };
                await context.Products.AddAsync(product);
                //await context.SaveChangesAsync();

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
                //await context.SaveChangesAsync();

                for (int i = 0; i < model.SubCategoryId.Count; i++)
                {
                    ProductSubCategory productSubCategory = new()
                    {
                        ProductId = product.Id,
                        SubCategoryId = model.SubCategoryId[i]
                    };
                    await context.AddAsync(productSubCategory);

                }
                //await context.SaveChangesAsync();

                foreach (var item in model.AddSpecificationDTOs)
                {
                    Specification specification = new();
                specification.ProductId = product.Id;
                await context.Specifications.AddAsync(specification);
                //await context.SaveChangesAsync();


                foreach (var specLang in item.AddSpecificationLanguageDTOs)
                    {
                        SpecificationLanguage specificationLanguage = new()
                        {
                            SpecificationId = specification.Id,
                            LangCode = specLang.LangCode,
                            Key = specLang.Key,
                            Value = specLang.Value
                        };
                    await context.SpecificationLanguages.AddAsync(specificationLanguage);
                }
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

            }
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDTO model)   
        {
           await using var context= new AppDbContext();
           var findData= context.Products
                .Include(x=>x.Specifications)
                .Include(x => x.ProductLanguages)
                .Include(x => x.ProductSubCategories)
                .ThenInclude(x => x.SubCategory)
                .FirstOrDefault(x => x.Id == id);

            findData.Discount = model.Discount;
            findData.IsStock = model.IsStock;
            findData.Price = model.Price;
            findData.Review = model.Review;
            context.Update(findData);

            context.Remove(findData.Specifications);
            context.Remove(findData.ProductLanguages);
            context.Remove(findData.ProductSubCategories);
            await context.SaveChangesAsync();

            foreach (var item in model.UpdateProductLanguageDTOs)
            {
                ProductLanguage productLanguage = new()
                {
                    ProductId = findData.Id,
                    ProductName = item.ProductName,
                    Description = item.Description,
                    LangCode = item.LangCode
                };
                await context.AddAsync(productLanguage);
            }
            await context.SaveChangesAsync();
            foreach (var item in model.UpdateProductLanguageDTOs)
            {
                Specification specification = new();
                specification.ProductId = findData.Id;
                await context.AddAsync(specification);
                SpecificationLanguage specificationLanguage = new()
                {
                    SpecificationId = specification.Id,
                    LangCode = item.LangCode,
                    Key = item.Key,
                    Value = item.Value
                };
            }
        }
    }
}
