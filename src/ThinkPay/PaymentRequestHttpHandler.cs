using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace ThinkPay
{
    /// <summary>
    /// 支付请求
    /// </summary>
    public class PaymentRequestHttpHandler : IHttpHandler
    {
        /// <summary>
        /// 支付网关
        /// </summary>
        public Gateway Gateway { get; private set; }
        /// <summary>
        /// 支付信息
        /// </summary>
        public PaymentInfo PaymentInfo { get; private set; }


        protected bool CheckGatewayAndOrder(string gateway, string orderId, out string message)
        {
            this.Gateway = GatewayManagement.Instance.Get(gateway);
            if(this.Gateway == null) {
                message = string.Format("The payment gateway of '{0}' is not found.", Gateway);
                return false;
            }

            this.PaymentInfo = BillService.Current.GetPaymentInfo(orderId);
            if(this.PaymentInfo == null) {
                message = string.Format("The order(no:{0}) is not found.", orderId);
                return false;
            }
            if(this.PaymentInfo.Amount <= 0) {
                message = string.Format("The order(no:{0}) amount is not found.", orderId);
                return false;
            }

            if(this.PaymentInfo.Prepaid) {
                message = string.Format("The order(no:{0}) was paid.", orderId);
                return false;
            }
            if(this.PaymentInfo.Closed) {
                message = string.Format("The order(no:{0}) has been closed.", orderId);
                return false;
            }

            message = null;
            return true;
        }
        
        public virtual void ProcessRequest(HttpContext httpContext)
        {
            var gateway = httpContext.Request.QueryString["gateway"];
            var orderId = httpContext.Request.QueryString["orderid"];

            string message = null;
            if(string.IsNullOrEmpty(gateway)) {
                message = "url parameters not contain gateway";
            }
            if(string.IsNullOrEmpty(orderId)) {
                message = "url parameters not contain orderId";
            }

            if(string.IsNullOrEmpty(message) || !CheckGatewayAndOrder(gateway, orderId, out message)) {
                Trace.WriteLine(message, "ThinkPay");
                httpContext.Response.Write(message);
                return;
            }


            var proxy = (IHttpProxy)Activator.CreateInstance(this.Gateway.PaymentRequestType,
                this.PaymentInfo, this.Gateway.Parameters);

            proxy.Render(new HttpContextWrapper(httpContext));          
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}
