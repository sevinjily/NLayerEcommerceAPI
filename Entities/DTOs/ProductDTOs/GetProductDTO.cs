﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class GetProductDTO
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsStock { get; set; }
        public double Review { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
        public List<string> SubCategoryName { get; set; }
        public List<GetSpecificationDTO> GetSpecificationDTOs { get; set; }
    }
}
