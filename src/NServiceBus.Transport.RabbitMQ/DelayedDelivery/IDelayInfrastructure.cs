namespace NServiceBus.Transport.RabbitMQ.DelayedDelivery
{
    using global::RabbitMQ.Client;

    /// <summary>
    /// Infrastructure for delaying message delivery
    /// </summary>
    public interface IDelayInfrastructure
    {
        /// <summary>
        /// Maximum allowed delay
        /// </summary>
        int MaxDelayInSeconds { get; }

        /// <summary>
        /// The header name in which the delay is specified
        /// </summary>
        string DelayHeader { get; }

        /// <summary>
        /// The delivery exchange
        /// </summary>
        string DeliveryExchange { get; }

        /// <summary>
        /// The binding key for the address
        /// </summary>
        string BindingKey(string address);

        /// <summary>
        /// Sends a message in delay to the specified address
        /// </summary>
        /// <param name="channel">The channel on which the message will be published</param>
        /// <param name="message">The message to publish</param>
        /// <param name="properties">Message properties</param>
        /// <param name="delayValue">Configured delay in seconds</param>
        /// <param name="address">Routing key for the delayed message</param>
        void SendMessageInDelay(IModel channel, OutgoingMessage message, IBasicProperties properties, int delayValue, string address);

        /// <summary>
        /// Creates the needed exchanges, queues and bindings for the infrastructure
        /// </summary>
        /// <param name="channel"></param>
        void Build(IModel channel);

        /// <summary>
        /// Deletes the exchanges, queues and bindings that were created for the infrastructure
        /// </summary>
        /// <param name="channel">The channel on which the message will be published</param>
        void TearDown(IModel channel);
    }
}
