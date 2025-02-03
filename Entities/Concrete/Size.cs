using Entities.Common;

namespace Entities.Concrete
{
    public class Size:BaseEntity
    {
        // Name of the size (e.g., "Small", "Medium", "Large")  
        public string Name { get; set; }

        // Code for the size (e.g., "S", "M", "L", "42")
        public string Code { get; set; }

        // Unit of measurement (e.g., "cm", "inch", "EU", "US")
        public string Unit { get; set; }

        // Numeric value for size dimension (e.g., 42.5 for shoes, or 250 for ml or grams)
        public decimal? Dimension { get; set; } // Nullable if not always needed

        // Optionally you can add a description for the size
        public string? Description { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
