using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIIdentity.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        //public Category ParentCategory { get; set; }
        //public ICollection<Product> Products { get; set; }
        public ICollection<CategoryProduct> CategoryProducts { get; set; }
        public ICollection<CategoryCategory> CategoryCategories1 { get; set; }
        public ICollection<CategoryCategory> CategoryCategories2 { get; set; }

    }
}
