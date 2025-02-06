using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class AddProductDTO
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsStock { get; set; }
        public double Review { get; set; }
        public List<Guid> SubCategoryId { get; set; }
        public List<AddProductLanguageDTO> AddProductLanguageDTOs { get; set; }
        public List<AddSpecificationDTO> AddSpecificationDTOs { get; set; }
    }
}
