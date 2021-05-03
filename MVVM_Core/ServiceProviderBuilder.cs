using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core
{

    public class ServiceProviderBuilder
    {
        IServiceCollection services = new ServiceCollection();


        public ServiceProviderBuilder UseStartup<T>() where T: new()
        {
            services.AddSingleton<PageService>();
            services.AddSingleton<EventBus>();

            var types = FindTypesByBaseClass("BaseViewModel");

            foreach (Type type in types)
            {
                
                var sn = type.GetCustomAttribute<Singleton>();

                if(sn != null)
                {
                    services.AddSingleton(type);

                }
                else
                {
                    services.AddTransient(type);
                }

            }

            var startupType = typeof(T);
            var m = startupType.GetMethod("ConfigureServices");

            if(m == null)
            {
                throw new ArgumentException($"Type {startupType.Name} does`not contains \"ConfigureServices\" method ");
            }

            T startup = new T();
          
            var @params = m.GetParameters();

            var obj = new List<object>();
            
            foreach (ParameterInfo parameter in @params)
            {
                if(services.Any(x => x.ServiceType == parameter.ParameterType))
                {
                    obj.Add(services.First(x => x.ServiceType == parameter.ParameterType));
                }
                else if(parameter.ParameterType == typeof(IServiceCollection))
                {
                    obj.Add(services);
                }
                else
                {
                    throw 
                        new ArgumentException($"Services does`not contains type \"{parameter.ParameterType}\" for \"ConfigureServices\" method");
                }


            }
            m.Invoke(startup, obj.ToArray());
            return this;
        }

        public IServiceProvider BuidSeriveProvider()
        {
            return services.BuildServiceProvider();
        }


        private Type[] FindTypesByBaseClass(string interfaceName)
        {
            Assembly current = Assembly.GetEntryAssembly();
            string name = current.GetName().Name;

            var types = current.GetTypes().
                Where(x => !x.IsAbstract && 
                x.IsClass &&
                x.GetInterfaces().Contains(typeof(INotifyPropertyChanged))).ToArray();

            if(types == null || types.Length == 0)
            {
                throw new ArgumentException("Assembly does`not contains any not-abstract types, that implement 'INotifyPropertyChanged' interface");
            }

            return types;
        }
    }
}
