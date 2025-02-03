using Entities.Common;

namespace Entities.Concrete
{
    public class ProductColor:BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ColorId { get; set; }
        public Color Color { get; set; }
    }
}
