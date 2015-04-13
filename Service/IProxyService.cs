using System;
using System.Threading.Tasks;

namespace AriProxy.Service
{
    /// <summary>
    /// Service end of the ARI proxy.
    /// </summary>
    public interface IProxyService
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Init();

        /// <summary>
        /// Called when channel is created (stasisStart).
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="ariBody">The ARI body.</param>
        /// <returns></returns>
        Task<string> ChannelConnected(string channelId, string ariBody);

        /// <summary>
        /// Occurs when command received from client.
        /// </summary>
        event EventHandler<CommandReceivedEventArgs> CommandReceived;

        /// <summary>
        /// Posts the event to the proxy message bus.
        /// </summary>
        /// <param name="ariBody">The ARI event body.</param>
        void PostEvent(string ariBody);

        /// <summary>
        /// Posts the command response to the proxy message bus.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="body">The command response body.</param>
        void PostCommandResponse(string clientId, string body);
    }

    /// <summary>
    /// Arguments for <see cref="IProxyService.CommandReceived"/> event.
    /// </summary>
    public class CommandReceivedEventArgs : EventArgs
    {
        public string ClientId { get; private set; }

        public string Body { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="clientId">The source client identifier.</param>
        /// <param name="body">The command body.</param>
        public CommandReceivedEventArgs(string clientId, string body)
        {
            ClientId = clientId;
            Body = body;
        }
    }
}
