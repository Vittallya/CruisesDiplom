using DAL;
using Main.Events;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Main.Services
{
    public class UpdateHandlerService
    {
        private readonly AllDbContext allDbContext;
        private readonly EventBus eventBus;

        public UpdateHandlerService(DAL.AllDbContext allDbContext, EventBus eventBus)
        {
            this.allDbContext = allDbContext;
            this.eventBus = eventBus;
        }

        public async void Handle(string message)
        {
            bool hasId = message.Contains("/");
            object id = null;

            string modelTypeName = hasId ? message.Split('/')[0] : message;

            string modelName1 = modelTypeName.Substring(modelTypeName.IndexOf('.') + 1);

            var ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("DAL"));

            Type type = ass.GetType(modelTypeName);
            var set = allDbContext.Set(type);

            if (hasId)
            {
                id = int.Parse(message.Split('/')[1]);
                var entity = set.Find(id);
                await allDbContext.Entry(entity).ReloadAsync();
            }
            //else
            //{
            //    await set.ForEachAsync(x => allDbContext.Entry(x).Reload());
            //}

            var eventType = typeof(DataUpdated<>).MakeGenericType(type);

            var instance = Activator.CreateInstance(eventType) as IEvent;

            await eventBus.Publish(instance);

        }
    }
}
