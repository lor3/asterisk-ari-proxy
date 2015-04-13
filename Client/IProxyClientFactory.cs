namespace AriProxy.Client
{
    interface IProxyClientFactory
    {
        IProxyClient CreateProxyClient(string applicationName);
    }
}
