using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using DAL;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Main
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<AllDbContext>();
            services.AddSingleton<Services.SplashScreenService>();
            services.AddSingleton<ConnectorService>();
            services.AddTransient<DbContextLoader>();
            services.AddTransient<LoginService>();
            services.AddTransient<MapperService>();
            services.AddSingleton<InsurancesService>();
            services.AddSingleton<RegisterService>();
            services.AddSingleton<ClientPipeHanlder>();
            services.AddTransient<Services.UpdateHandlerService>();
            services.AddSingleton<BL.OrderService>();
            services.AddSingleton<BL.ToursService>();
            services.AddSingleton<BL.SourceService>();
            services.AddSingleton<UserService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<WordService>();
            services.AddSingleton<ValuteGetterService>();
            services.AddSingleton<Services.PassengersService>();
            services.AddSingleton<PlacementService>();
            services.AddTransient<IConfiguration>(x => (new ConfigurationBuilder().
                        AddInMemoryCollection(new Dictionary<string, string>() 
                        {
                            { "DefaultImagePath", Path.Combine(Environment.CurrentDirectory, "Images") },
                            { "WordFile", Path.Combine(Environment.CurrentDirectory, "obrazec.docx") },
                        }).Build()));
        }
    }
}
