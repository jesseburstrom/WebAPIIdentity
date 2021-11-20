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
        public double Price { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
