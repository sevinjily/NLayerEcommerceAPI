using Entities.Common;

namespace Entities.Concrete
{
    public class Color:BaseEntity
    {
        public string Name { get; set; }
        public string HexCode { get; set; }  // Stores color as hex code (e.g., #FF0000 for red)
    }
}
