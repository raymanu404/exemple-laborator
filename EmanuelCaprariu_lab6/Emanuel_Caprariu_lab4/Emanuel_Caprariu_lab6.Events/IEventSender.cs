using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;

namespace Emanuel_Caprariu_lab6.Events
{
    public interface IEventSender
    {
        TryAsync<Unit> SendAsync<T>(string topicName, T @event);
    }
}
