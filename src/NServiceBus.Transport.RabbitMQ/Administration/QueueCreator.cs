namespace NServiceBus.Transport.RabbitMQ
{
    using System.Threading.Tasks;
    using DelayedDelivery;

    class QueueCreator : ICreateQueues
    {
        readonly ConnectionFactory connectionFactory;
        readonly IRoutingTopology routingTopology;
        readonly IDelayInfrastructure delayInfrastructure;

        public QueueCreator(ConnectionFactory connectionFactory, IRoutingTopology routingTopology, IDelayInfrastructure delayInfrastructure)
        {
            this.connectionFactory = connectionFactory;
            this.routingTopology = routingTopology;
            this.delayInfrastructure = delayInfrastructure;
        }

        public Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            using (var connection = connectionFactory.CreateAdministrationConnection())
            using (var channel = connection.CreateModel())
            {
                delayInfrastructure.Build(channel);

                routingTopology.Initialize(channel, queueBindings.ReceivingAddresses, queueBindings.SendingAddresses);

                foreach (var receivingAddress in queueBindings.ReceivingAddresses)
                {
                    routingTopology.BindToDelayInfrastructure(channel, receivingAddress, delayInfrastructure.DeliveryExchange, delayInfrastructure.BindingKey(receivingAddress));
                }
            }

            return TaskEx.CompletedTask;
        }
    }
}