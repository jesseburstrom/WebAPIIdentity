using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIIdentity.Areas.Identity.Data;

namespace WebAPIIdentity.Models
{
    public class OrderList
    {
        public int OrderListId { get; set; }
        public WebAPIIdentityUser UserId { get; set; }
    }
}
