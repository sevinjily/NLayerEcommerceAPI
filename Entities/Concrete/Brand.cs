using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Brand :BaseEntity
    {
        public string BrandName { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
    }
}
