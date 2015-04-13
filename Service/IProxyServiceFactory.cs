namespace AriProxy.Service
{
    public interface IProxyServiceFactory
    {
        IProxyService CreateProxyService(string applicationName);
    }
}
