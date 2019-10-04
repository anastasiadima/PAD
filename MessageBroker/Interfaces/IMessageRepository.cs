using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Interfaces
{
     public interface IMessageRepository: IRepository<Message,int>
     {
     }
}
