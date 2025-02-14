using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.BrandDTOs
{
    public class AddBrandDTO
    {
        public string BrandName { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
    }
}
