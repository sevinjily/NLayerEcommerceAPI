using Entities.Common;

namespace Entities.Concrete
{
    public class Category:BaseEntity
    {
        
        public List<CategoryLanguage> CategoryLanguages { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}
