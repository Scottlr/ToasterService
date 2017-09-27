using Orleans.Runtime.Configuration;
using System;
using System.Linq;
using System.Net;

namespace Orleans.AspNetCore
{
    public class OrleansHostBuilder : IOrleansHostBuilder
    {
        private Type startupType;

        public IOrleansHost Build()
        {
            var host = new OrleansHost();

            var config = host.Config;
            DefaultConfig(config);

            if (startupType == null)
                throw new NullReferenceException("Startup undefined.");

            var startup = (IOrleansStartup)Activator.CreateInstance(startupType);
            startup.ConfigureOrleans(config);

            return host;
        }

        private static void DefaultConfig(ClusterConfiguration config)
        {
			config.Defaults.SiloName = "default";
			config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.ZooKeeper;
			config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.Disabled;
			config.Globals.DataConnectionString = "zookeeper:2181";
			config.Defaults.ProxyGatewayEndpoint = new IPEndPoint(IPAddress.Any, 30000);
			config.Defaults.Port = 33333;
			config.Globals.RegisterStorageProvider("Orleans.Storage.MemoryStorage", "Default");
			var ips = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result;
			config.Defaults.HostNameOrIPAddress = ips.FirstOrDefault()?.ToString();
			config.Defaults.PropagateActivityId = true;
		}

        public IOrleansHostBuilder UseStartup<TStartup>() where TStartup : IOrleansStartup
        {
            startupType = typeof(TStartup);
            return this;
        }
    }
}
