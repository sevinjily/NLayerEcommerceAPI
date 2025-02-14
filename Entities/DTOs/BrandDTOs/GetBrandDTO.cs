using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.BrandDTOs
{
    public class GetBrandDTO
    {
        public Guid Id { get; set; }
        public bool IsDeleted{ get; set; }
        public DateTime CreatedDate { get; set; }
        public string BrandName { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
    }
}
