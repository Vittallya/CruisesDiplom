using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Core
{
    public class EventBus
    {
        private ConcurrentDictionary <EventSubscriber, Func<IEvent, Task>> subscribers;

        public EventBus()
        {
            subscribers = new ConcurrentDictionary<EventSubscriber, Func<IEvent, Task>>();
        }

        public IDisposable Subscribe<T, TSub>(Func<T, Task> func, bool isOnce = true) where T: IEvent
        {   
            var same = subscribers.Keys.FirstOrDefault(x => x.MesType == typeof(T) && x.Sub == typeof(TSub));
            
            if(same != null)
            {
                return null;
            }

            var disposableObj = new EventSubscriber(typeof(T), typeof(TSub), d => subscribers.TryRemove(d, out var _), isOnce);

            subscribers.TryAdd(disposableObj, element => func((T)element));
            
            return disposableObj;
        }

        public void Describe<T, TSub>() where T : IEvent
        {
            var same = subscribers.Keys.FirstOrDefault(x => x.MesType == typeof(T) && x.Sub == typeof(TSub));

            same?.Dispose();
        }

        public async Task Publish<T>(T message) where T: IEvent
        {
            var messType = message.GetType();

            var subs = subscribers.Where(x => x.Key.MesType == messType);

            var tasks = subs
                .Select(y => y.Value(message));

            await Task.WhenAll(tasks);

            subs.
                Select(x => x.Key).
                Where(x => x.IsOnce).
                ToList().
                ForEach(x => x.Dispose());

        }
    }
}
