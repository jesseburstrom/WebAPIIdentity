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
        public string Description { get; set; }
        public string DescriptionSwedish { get; set; }
        public int ImageId { get; set; }
        private ICollection<CategoryCategory> CategoryCategories1 { get; set; }
        private ICollection<CategoryCategory> CategoryCategories2 { get; set; }
        private ICollection<CategoryProduct> CategoryProducts { get; set; }

    }
}
