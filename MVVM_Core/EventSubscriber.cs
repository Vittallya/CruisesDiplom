using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_Core
{
    public class EventSubscriber : IDisposable
    {

        private readonly Action<EventSubscriber> action;

        public bool HasGenericType { get; }

        public Type MesType { get; }
        public Type GenericType { get; }
        public Type Sub { get; }
        public bool IsOnce { get; }

        public EventSubscriber(Type mesType, Type sub, Action<EventSubscriber> action, bool isOnce)
        {
            MesType = mesType;
            Sub = sub;
            this.action = action;
            IsOnce = isOnce;
            HasGenericType = mesType.IsGenericType;

            if (HasGenericType)
            {
                GenericType = mesType.GetGenericArguments()[0];
            }

        }
        public void Dispose()
        {
            action?.Invoke(this);
        }
    }
}
