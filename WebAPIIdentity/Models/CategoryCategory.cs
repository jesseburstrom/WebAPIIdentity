using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIIdentity.Models
{
    public class CategoryCategory
    {
        public int CategoryCategoryId { get; set; }
        public int CategoryId1 { get; set; }
        public Category Category1 { get; set; }
        public int CategoryId2 { get; set; }
        public Category Category2 { get; set; }

        //public int CategoryCategoryId { get; set; }
    }
}
