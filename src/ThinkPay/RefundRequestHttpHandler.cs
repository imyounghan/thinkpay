using System;
using System.Diagnostics;
using System.Web;


namespace ThinkPay
{
    public class RefundRequestHttpHandler : IHttpHandler
    {
        /// <summary>
        /// 支付网关
        /// </summary>
        public Gateway Gateway { get; private set; }
        /// <summary>
        /// 退款信息
        /// </summary>
        public RefundInfo RefundInfo { get; private set; }

        public virtual void ProcessRequest(HttpContext httpContext)
        {
            var refundId = httpContext.Request.QueryString["refundid"];

            string message = null;
            if(string.IsNullOrEmpty(refundId)) {
                message = "url parameters not contain refundid";
            }

            if(string.IsNullOrEmpty(message) || !CheckRefundGateway(refundId, out message)) {
                Trace.WriteLine(message, "ThinkPay");
                httpContext.Response.Write(message);
                return;
            }


            var proxy = (IHttpProxy)Activator.CreateInstance(this.Gateway.RefundRequestType,
                this.RefundInfo, this.Gateway.Parameters);

            proxy.Render(new HttpContextWrapper(httpContext));
        }

        protected bool CheckRefundGateway(string refundId, out string message)
        {
            this.RefundInfo = BillService.Current.GetRefundInfo(refundId);

            if(this.RefundInfo == null) {
                message = string.Format("The refund order(no:{0}) is not found.", refundId);
                return false;
            }

            if(this.RefundInfo.Rejected) {
                message = string.Format("The refund order({0}) was refused", refundId);
                return false;
            }

            if(this.RefundInfo.Completed) {
                message = string.Format("The refund order({0}) has been completed", refundId);
                return false;
            }

            if(string.IsNullOrEmpty(this.RefundInfo.To)) {
                message = string.Format("The refund order({0}) not source to payment.", refundId);
                return false;
            }

            this.Gateway = GatewayManagement.Instance.Get(this.RefundInfo.To);
            if(this.Gateway == null) {
                message = string.Format("The payment gateway of '{0}' is not found.", Gateway);
                return false;
            }

            message = null;
            return true;
        }
        

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}
