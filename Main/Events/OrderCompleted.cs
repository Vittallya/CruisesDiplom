using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Events
{
    public class OrderCompleted: IEvent
    {
        public OrderCompleted(string serviceName, int result = 0)
        {
            ServiceName = serviceName;
            Result = result;
        }

        public string ServiceName { get; set; }

        /// <summary>
        /// Результат обработки заказа: 0 - успех, 1 - неуспех
        /// </summary>
        public int Result { get; set; }
    }
}
