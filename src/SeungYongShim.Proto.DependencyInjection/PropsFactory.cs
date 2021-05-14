using System;
using Microsoft.Extensions.DependencyInjection;
using Proto;

namespace SeungYongShim.Proto.DependencyInjection
{
    internal class PropsFactory<T> : IPropsFactory<T> where T : IActor
    {
        public PropsFactory(IRootContext rootContext, IServiceProvider serviceProvider)
        {
            RootContext = rootContext;
            ServiceProvider = serviceProvider;
        }

        public IRootContext RootContext { get; }
        public IServiceProvider ServiceProvider { get; }

        public Props Create(params object[] args)
            => Props.FromProducer(() => (T)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(T), args));
    }
}
