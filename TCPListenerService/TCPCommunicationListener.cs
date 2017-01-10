using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System.Threading.Tasks;
using System.Threading;
using System.Fabric;
using System.Net.Sockets;
using System.Net;

namespace TCPListenerService
{
    public class TCPCommunicationListener : ICommunicationListener
    {
        private readonly StatelessServiceContext _serviceContext;
        //you can expose this to another class, or internally open a socket and process byte arrays in here then pass out an Action or Func.
        private readonly TcpListener _tcpListener;
        private readonly int _port;

        /// <summary>
        /// Create the communication listener, but don't open the connection just yet.
        /// </summary>
        /// <param name="serviceContext">The service context is used to extract information from the endpoint.</param>
        public TCPCommunicationListener(StatelessServiceContext serviceContext)
        {
            _serviceContext = serviceContext;

            //TCPListenerEndpoint matches the name of the listener in ServiceManiest.xml.
            _port = serviceContext.CodePackageActivationContext.GetEndpoint("TCPListenerEndpoint").Port;
            //Listen on any IP, but the specified port.
            _tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, _port));
        }

        /// <summary>
        /// "Hard" abort
        /// </summary>
        public void Abort()
        {
            StopTcpListener();
        }

        /// <summary>
        /// "Soft" abort/close. Cancellation token should be ignorable for this.
        /// </summary>
        /// <param name="cancellationToken">Ignored</param>
        /// <returns>An empty Task.</returns>
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            StopTcpListener();

            return Task.FromResult(true);
        }

        //Start the communication listener and return the published URI. Could handle the cancellation token here.
        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            ServiceEventSource.Current.ServiceMessage(_serviceContext, "Starting TCP listener on port {0}", _port);
            _tcpListener.Start();

            //generate published URI from IP/FQDN and port number.
            string uriPublished = $"tcp:{FabricRuntime.GetNodeContext().IPAddressOrFQDN}:{_port}";

            ServiceEventSource.Current.ServiceMessage(_serviceContext, "TCP listener started and published with address {0}", uriPublished);

            return Task.FromResult(uriPublished);
        }

        /// <summary>
        /// Reusable method to stop the TCP listener. Used by CloseAsync and Abort.
        /// </summary>
        private void StopTcpListener()
        {
            ServiceEventSource.Current.ServiceMessage(_serviceContext, "Stopping TCP listener on port {0}", _port);

            _tcpListener.Stop();

            ServiceEventSource.Current.ServiceMessage(_serviceContext, "TCP listener stopped.");
        }
    }
}
