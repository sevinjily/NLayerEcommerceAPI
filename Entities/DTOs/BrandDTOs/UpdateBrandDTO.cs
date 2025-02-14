using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DTOs.BrandDTOs
{
    public class UpdateBrandDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
    }
}
