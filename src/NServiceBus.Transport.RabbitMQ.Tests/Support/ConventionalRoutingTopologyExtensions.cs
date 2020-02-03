using System.Collections.Generic;
using System.Linq;

namespace NServiceBus.Transport.RabbitMQ.Tests.Support
{
    using RabbitMQ.DelayedDelivery;

    static class ConventionalRoutingTopologyExtensions
    {
        public static void Reset(
            this ConventionalRoutingTopology routingTopology,
            ConnectionFactory connectionFactory,
            IDelayInfrastructure delayInfrastructure,
            IEnumerable<string> receivingAddresses,
            IEnumerable<string> sendingAddresses)
        {
            using (var connection = connectionFactory.CreateAdministrationConnection())
            using (var channel = connection.CreateModel())
            {
                foreach (var address in receivingAddresses.Concat(sendingAddresses))
                {
                    channel.QueueDelete(address, false, false);
                    channel.ExchangeDelete(address, false);
                }

                delayInfrastructure.TearDown(channel);
                delayInfrastructure.Build(channel);

                routingTopology.Initialize(channel, receivingAddresses, sendingAddresses);
            }
        }
    }
}
