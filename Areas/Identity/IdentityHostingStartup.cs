using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(DivineMonad.Areas.Identity.IdentityHostingStartup))]
namespace DivineMonad.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}