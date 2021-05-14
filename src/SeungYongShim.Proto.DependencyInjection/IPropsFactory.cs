using Proto;

namespace SeungYongShim.Proto.DependencyInjection
{
    public interface IPropsFactory<out TActor> where TActor : IActor
    {
        Props Create(params object[] args);
    }
}
