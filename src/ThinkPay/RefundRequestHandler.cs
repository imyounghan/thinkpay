using System;
using System.Web;


namespace ThinkPay
{
    public class RefundRequestHandler : IHttpHandler
    {
        public string RefundId { get; private set; }


        protected virtual void ProcessRequest(HttpContextBase httpContext)
        {
            var order = BillService.Instance.GetRefundInfo(RefundId);
            string message;
            if (!CheckOrder(order, out message)) {
                httpContext.Response.Write(message);
                return;
            }

            var gateway = GatewayManagement.Instance.Get(order.PayGateway);
            if (gateway == null) {
                httpContext.Response.Write("payment gateway is not found");
                return;
            }

            var proxy = (IHttpProxy)Activator.CreateInstance(Type.GetType(gateway.RefundRequestTypeName), order, gateway.Parames);

            proxy.Render(httpContext);
        }

        bool CheckOrder(IRefund order, out string message)
        {
            if (order == null) {
                message = string.Format("refund({0}) is not found", order.RefundNo);
                return false;
            }
            if (order.Rejected) {
                message = string.Format("refund({0}) was refused", order.RefundNo);
                return false;
            }
            if (order.Completed) {
                message = string.Format("refund({0}) has been completed", order.RefundNo);
                return false;
            }

            message = string.Empty;
            return true;
        }


        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            RefundId = context.Request.QueryString["refundId"];

            this.ProcessRequest(new HttpContextWrapper(context));

            context.Response.End();
        }
    }
}
