using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsStock { get; set; }
        public double Review { get; set; }
        public IList<Guid> SubCategoryId { get; set; }
        public List<UpdateProductLanguageDTO> UpdateProductLanguageDTOs { get; set; }
        public List<UpdateSpecificationDTO> UpdateSpecificationDTOs { get; set; }
    }
}
