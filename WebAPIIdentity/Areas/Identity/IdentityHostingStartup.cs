using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPIIdentity.Areas.Identity.Data;
using WebAPIIdentity.Data;

[assembly: HostingStartup(typeof(WebAPIIdentity.Areas.Identity.IdentityHostingStartup))]
namespace WebAPIIdentity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WebAPIIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WebAPIIdentityContextConnection")));

                services.AddDefaultIdentity<WebAPIIdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<WebAPIIdentityContext>();
            });
        }
    }
}