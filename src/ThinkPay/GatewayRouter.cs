using System;
using System.Threading;
using System.Web;

using ThinkNet.Components;


namespace ThinkNet.Payment
{
    //[NonRegistered]
    public sealed class GatewayRouter : IGatewayRouter
    {
        public readonly static GatewayRouter Instance = new GatewayRouter();


        private Func<IGatewayRouter> _provider = delegate {
            if (!ObjectContainer.Instance.IsRegistered<IGatewayRouter>()) {
                throw new Exception("Not found the implementation class of IGatewayRouter.");
            }
            return ObjectContainer.Instance.Resolve<IGatewayRouter>();
        };
        public void SetProvider(Func<IGatewayRouter> provider)
        {
            Ensure.NotNull(provider, "provider");
            _provider = provider;
        }

        private IGatewayRouter _cacheRouter = null;
        private IGatewayRouter GetRouter()
        {
            if (_cacheRouter != null)
                return _cacheRouter;

            return Interlocked.CompareExchange<IGatewayRouter>(ref _cacheRouter,
                _provider(), null);
        }

        public string Route(HttpRequestBase httpRequest)
        {
            return GetRouter().Route(httpRequest);
        }
    }
}
