using System;
using System.Collections;
using System.Web;
using Microsoft.Practices.ServiceLocation;


namespace ThinkPay
{
    public class PaymentNotifyHandler : IHttpHandler
    {
        public string GatewayName { get; private set; }

        private readonly IGatewayRouter _gatewayRouter;
        public PaymentNotifyHandler()
        {
            this._gatewayRouter = ServiceLocator.Current.GetInstance<IGatewayRouter>();
        }

        protected virtual void ProcessRequest(HttpContextBase httpContext)
        {
            var gateway = GatewayManagement.Instance.Get(GatewayName);

            if (gateway == null) {
                throw new HttpException(500, "payment gateway is not found.");
            }

            Hashtable ht = new Hashtable(gateway.Parames);
            //for (int index = 0; index < httpContext.Request.Form.Count; index++) {
            //    ht[httpContext.Request.Form.GetKey(index)] = httpContext.Request.Form.Get(index);
            //}
            foreach (var key in httpContext.Request.Form.AllKeys) {
                ht.Add(key, httpContext.Request.GetFormString(key));
            }

            var proxy = (IHttpProxy)Activator.CreateInstance(Type.GetType(gateway.PaymentNotifyTypeName), ht);

            proxy.Render(httpContext);
        }

        

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var httpContext = new HttpContextWrapper(context);

            GatewayName = _gatewayRouter.Route(httpContext.Request);

            this.ProcessRequest(httpContext);

            context.Response.End();
        }
    }
}
