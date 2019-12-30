namespace Etdb.ReportingService
{
    public class AzureServiceBusConfiguration
    {
        public string ConnectionString { get; set; } = null!;

        public string UserRegisteredTopic => "user-registered";

        public string UserAuthenticatedTopic => "user-authenticated";
    }
}