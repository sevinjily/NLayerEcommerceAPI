using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
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

                foreach (var item in model.AddProductPicturesDTOs)
                {
                    ProductPicture productPicture = new()
                    {
                       
                        FileName = item.FileName,
                        Path = item.Path,
                        ProductId = product.Id
                    };
                    await context.ProductPictures.AddAsync(productPicture);
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

            }
        }

        public GetProductDTO GetProductAsync(Guid id, string LangCode)
        {
            using var context = new AppDbContext();
            var data = context.Products
                .Where(x => x.Id == id && x.IsDeleted == false)
                .Include(x => x.ProductLanguages)
                .Include(x => x.ProductSubCategories)
                .ThenInclude(x => x.SubCategory)
                .Include(x => x.Specifications)
                .ThenInclude(x => x.SpecificationLanguages)
                 .FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
            GetProductDTO getProductDTO = new()
            {
                LangCode = data.ProductLanguages.FirstOrDefault(x => x.LangCode == LangCode).LangCode,
                Description = data.ProductLanguages.FirstOrDefault(x => x.LangCode == LangCode).Description,
                ProductName = data.ProductLanguages.FirstOrDefault(x => x.LangCode == LangCode).ProductName,
                Discount = data.Discount,
                IsStock = data.IsStock,
                Price = data.Price,
                Review = data.Review,
                SubCategoryName = data.ProductSubCategories
                .Where(x => x.ProductId == data.Id)
                .Select(x => x.SubCategory.Name)
                .ToList(),
                GetSpecificationDTOs = data.Specifications.Select(x => new GetSpecificationDTO
                {
                    Key = x.SpecificationLanguages.FirstOrDefault(x => x.LangCode == LangCode).Key,
                    Value = x.SpecificationLanguages.FirstOrDefault(x => x.LangCode == LangCode).Value
                }).ToList()
            };
                return getProductDTO;
            }
                return null;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            await using var context = new AppDbContext();
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsDeleted = true; // Məhsulu aktivdən çıxarırıq
                await context.SaveChangesAsync();
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

            context.RemoveRange(findData.Specifications);
            context.RemoveRange(findData.ProductLanguages);
            context.RemoveRange(findData.ProductSubCategories);
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


            foreach (var item in model.UpdateSpecificationDTOs)
            {
                Specification specification = new();
                specification.ProductId = findData.Id;
                await context.AddAsync(specification);
                foreach (var spec in item.UpdateSpecificationLanguageDTOs)
                {
                    
                SpecificationLanguage specificationLanguage = new()
                {
                    SpecificationId = specification.Id,
                    LangCode = spec.LangCode,
                    Key = spec.Key,
                    Value = spec.Value 
                };
                    await context.AddAsync(specificationLanguage);
                }
                await context.SaveChangesAsync();
            }
            foreach (var item in model.SubCategoryId)
            {
                ProductSubCategory productSubCategory = new()
                {
                    ProductId = findData.Id,
                    SubCategoryId = item
                };
                await context.AddAsync(productSubCategory);
            }
            await context.SaveChangesAsync();
        }
    }
}
