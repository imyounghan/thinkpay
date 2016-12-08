using System;
using System.Collections;
using System.Diagnostics;
using System.Web;


namespace ThinkPay
{
    public class PaymentNotifyHttpHandler : IHttpHandler
    {
        public Gateway Gateway { get; private set; }


        public virtual void ProcessRequest(HttpContext httpContext)
        {
            this.Gateway = GatewayManagement.Instance.Get(httpContext.Request.UserHostAddress);
            if(this.Gateway == null) {
                var message = string.Format("The payment gateway from '{0}' is not found.", 
                    httpContext.Request.UserHostAddress);
                Trace.TraceError(message);
                httpContext.Response.Write(message);
                return;
            }

            Hashtable ht = new Hashtable(Gateway.Parameters);
            foreach(var key in httpContext.Request.Form.AllKeys) {
                ht.Add(key, httpContext.Request.Form[key]);
            }

            var proxy = (IHttpProxy)Activator.CreateInstance(Gateway.PaymentNotifyType, ht);

            proxy.Render(new HttpContextWrapper(httpContext));
        }

        

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}
