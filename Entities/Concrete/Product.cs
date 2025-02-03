using Entities.Common;

namespace Entities.Concrete
{
    public class Product:BaseEntity
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsStock { get; set; }
        public double Review { get; set; }
        public List<ProductLanguage> ProductLanguages { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public ICollection<ProductSubCategory> ProductSubCategories { get; set; }
    }
}
