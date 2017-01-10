using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace TCPListenerService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TCPListenerService : StatelessService
    {
        public TCPListenerService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Create our TCP listener
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            ServiceInstanceListener sil = new ServiceInstanceListener((serviceContext) =>
            {
                return new TCPCommunicationListener(serviceContext);
            });

                return new ServiceInstanceListener[] { sil };
        }
    }
}
