namespace Entities.Concrete
{
    public class ProductSize

    {
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
