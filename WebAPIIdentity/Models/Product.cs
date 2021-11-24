using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIIdentity.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string DescriptionSwedish { get; set; }
        public int Price { get; set; }
        public int ImageId { get; set; }
        private ICollection<CategoryProduct> CategoryProducts { get; set; }

    }
}
