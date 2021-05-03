using Microsoft.Extensions.DependencyInjection;
using BL;
using DAL;
using MVVM_Core;
using Admin.Services;

namespace Admin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<DAL.AllDbContext>();
            services.AddTransient<BL.DbContextLoader>();
            services.AddTransient<Services.CloneItemsSerivce>();
            services.AddSingleton<Services.FieldsGenerator>();
            services.AddSingleton<BL.ClientPipeHanlder>();
            services.AddSingleton<BL.ServerPipeHandler>();
        }
    }
}
