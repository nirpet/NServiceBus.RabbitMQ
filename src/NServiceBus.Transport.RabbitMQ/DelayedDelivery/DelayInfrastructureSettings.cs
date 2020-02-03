namespace NServiceBus.Transport.RabbitMQ.DelayedDelivery
{
    using Features;
    using Settings;

    static class DelayInfrastructureSettings
    {
        public const string XDeathHeader = "x-death";
        public const string XFirstDeathExchangeHeader = "x-first-death-exchange";
        public const string XFirstDeathQueueHeader = "x-first-death-queue";
        public const string XFirstDeathReasonHeader = "x-first-death-reason";


        public static StartupCheckResult CheckForInvalidSettings(SettingsHolder settings)
        {
            var timeoutManagerShouldBeEnabled = settings.GetOrDefault<bool>(SettingsKeys.EnableTimeoutManager);
            var timeoutManagerFeatureActive = settings.GetOrDefault<FeatureState>(typeof(TimeoutManager).FullName) == FeatureState.Active;

            if (timeoutManagerShouldBeEnabled && !timeoutManagerFeatureActive)
            {
                return StartupCheckResult.Failed("The transport has been configured to enable the timeout manager, but the timeout manager is not active." +
                                                 "Ensure that the timeout manager is active or remove the call to 'EndpointConfiguration.UseTransport<RabbitMQTransport>().DelayedDelivery().EnableTimeoutManager()'.");
            }

            return StartupCheckResult.Success;
        }

    }
}
