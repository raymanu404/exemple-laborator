using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Emanuel_Caprariu_lab6.Events
{
    public interface IEventListener
    {
        Task StartAsync(string topicName, string subscriptionName, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
