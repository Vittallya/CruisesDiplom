using DAL.Dto;
using MVVM_Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Events
{
    public class AccountEntered : IEvent
    {
        public ClientDto Client { get; set;  }

        public AccountEntered(ClientDto client)
        {
            Client = client;
        }
    }
}
