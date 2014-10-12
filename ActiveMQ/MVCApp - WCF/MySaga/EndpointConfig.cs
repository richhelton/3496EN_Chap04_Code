using System;
using NServiceBus;
using NLog;
using NServiceBus.Config;



namespace MySaga
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, IWantToRunWhenBusStartsAndStops
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
       
        public void Init()
        {

            // Log the Bus
            SetLoggingLibrary.NLog();

            UnicastBusConfig unicastBusCfg = Configure.GetConfigSection<UnicastBusConfig>();
            Logging loggingCfg = Configure.GetConfigSection<Logging>();
            TransportConfig transportCfg = Configure.GetConfigSection<TransportConfig>();
            SecondLevelRetriesConfig secondCfg = Configure.GetConfigSection<SecondLevelRetriesConfig>();
            AuditConfig auditCfg = Configure.GetConfigSection<AuditConfig>();
            MsmqSubscriptionStorageConfig endpoinsCfg = Configure.GetConfigSection<MsmqSubscriptionStorageConfig>();
  
   
            Configure.With()
                .DefaultBuilder()  // Autofac Default Container
                .UseTransport<NServiceBus.ActiveMQ>()  
                .InMemorySubscriptionStorage()
                .UseNHibernateSagaPersister()
                .UseNHibernateTimeoutPersister()
                .UnicastBus(); // Create the default unicast Bus

  
        
        }

        public void Start()
        {
            Console.WriteLine("This is the process hosting the saga.");
        }

        public void Stop()
        {
            Console.WriteLine("Stopped.");
        }
    }
}
