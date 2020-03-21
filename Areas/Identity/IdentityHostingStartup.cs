using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(asp_projekt.Areas.Identity.IdentityHostingStartup))]
namespace asp_projekt.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}