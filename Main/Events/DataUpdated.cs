using DAL;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Events;

namespace Main.Events
{


    public class DataUpdated<T>: IEvent
    {
        public int Id { get; set; }

       
    }
}
