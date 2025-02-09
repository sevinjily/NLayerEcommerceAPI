using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class UpdateProductLanguageDTO
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
    }
}
