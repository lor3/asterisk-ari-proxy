using System;
using AsterNET.ARI;
using AsterNET.ARI.Models;
using Newtonsoft.Json;
using RestSharp;

namespace AriProxy.Service
{
    public class AppConnector
    {
        #region Private fields

        private readonly IProxyServiceFactory _proxyServiceFactory;
        private readonly StasisEndpoint _endpoint;
        private readonly string _application;
        
        private IProxyService _proxyService;
        private AriClient _ariClient;
        private RestClient _restClient;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AppConnector"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="proxyServiceFactory">The proxy service factory.</param>
        /// <param name="application"></param>
        public AppConnector(IProxyServiceFactory proxyServiceFactory, StasisEndpoint endpoint, string application)
        {
            _endpoint = endpoint;
            _application = application;
            _proxyServiceFactory = proxyServiceFactory;

            CreateChannel();
            EstablishStatisConnection();
        }

        private void CreateChannel()
        {
            _proxyService = _proxyServiceFactory.CreateProxyService(_application);
            _proxyService.Init();
            _proxyService.CommandReceived += OnCommandReceived;
        }

        private void EstablishStatisConnection()
        {
            _ariClient = new AriClient(_endpoint, _application);
            _ariClient.OnUnhandledEvent += OnEventReceived;

            _restClient = new RestClient(_endpoint.AriEndPoint)
            {
                Authenticator = new HttpBasicAuthenticator(_endpoint.Username, _endpoint.Password)
            };
        }

        #region Event Handlers

        private void OnCommandReceived(object sender, CommandReceivedEventArgs args)
        {
            var command = JsonConvert.DeserializeObject<Command>(args.Body);

            var request = new RestRequest(command.Url, (Method) Enum.Parse(typeof (Method), command.Method));
            request.AddBody(command.Body);

            var result = _restClient.Execute(request);
            _proxyService.PostCommandResponse(null, JsonConvert.SerializeObject(new CommandResult
            {
                UniqueId = command.UniqueId,
                StatusCode = (int) result.StatusCode,
                ResponseBody = result.Content
            }));
        }

        private void OnEventReceived(IAriClient sender, Event eventMessage)
        {
            switch (eventMessage.Type.ToLower())
            {
                case "statisstart":
                {
                    var startArgs = (StasisStartEvent) eventMessage;

                    try
                    {
                        var clientId = _proxyService.ChannelConnected(startArgs.Channel.Id, JsonConvert.SerializeObject(eventMessage)).Result;

                        // set clientId as property of channel to assist with event filtering on client
                        
                    }
                    catch (Exception)
                    {
                        // close the connection ??
                    }

                    break;
                }

                default:
                {
                    if (eventMessage.Type.ToLower().StartsWith("bridge"))
                    {
                        // can we retrieve clientId from message to assist with event filtering on client
                    }
                    else if (eventMessage.Type.ToLower().StartsWith("channel"))
                    {
                        // can we retrieve clientId from message to assist with event filtering on client
                    }
                    
                    _proxyService.PostEvent(JsonConvert.SerializeObject(eventMessage));
                    break;
                }
            }
        }

        #endregion
    }
}
