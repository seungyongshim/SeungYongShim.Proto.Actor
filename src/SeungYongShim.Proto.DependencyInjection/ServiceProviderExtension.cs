using System;
using Proto.Extensions;

namespace Proto
{
    public class ServiceProviderExtension : IActorSystemExtension<ServiceProviderExtension>
    {
        public ServiceProviderExtension(IServiceProvider serviceProvider)
            => ServiceProvider = serviceProvider;

        public IServiceProvider ServiceProvider { get; }
    }
}
