using NServiceBus;
using NServiceBus.Features;
using System;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run().GetAwaiter().GetResult();
        }

        public async Task Run()
        {
            var bus = await SetupBus().ConfigureAwait(false);
            Console.ReadLine();
        }

        private async Task<IEndpointInstance> SetupBus()
        {
            EndpointConfiguration configuration = new EndpointConfiguration("DevelopmentConsole");
            var transport = configuration.UseTransport<MsmqTransport>();
            var scanner = configuration.AssemblyScanner();
            scanner.ScanAppDomainAssemblies = true;
            configuration.SendOnly();
            configuration.DisableFeature<Audit>();
            configuration.UsePersistence<InMemoryPersistence>();
            var endpoint = await Endpoint.Create(configuration).ConfigureAwait(false);
            return await endpoint.Start().ConfigureAwait(false);
        }
    }
}
