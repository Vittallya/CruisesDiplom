using BL;
using DAL;
using DAL.Dto;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Events
{
    public class ClientRegistered: IEvent
    {
        public ClientRegistered(ClientDto user)
        {
            User = user;
        }

        public ClientDto User { get; set; }

    }
}
