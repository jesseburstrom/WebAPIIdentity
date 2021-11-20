using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIIdentity.Models
{
    public class ProductOrder
    {
        public int ProductOrderId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
