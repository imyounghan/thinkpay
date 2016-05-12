using System;
using System.Web;

namespace ThinkPay
{
    public class PaymentRequestHandler : IHttpHandler
    {
        public string GatewayName { get; private set; }

        public string OrderId { get; private set; }


        
        protected virtual void ProcessRequest(HttpContextBase httpContext)
        {
            var order = BillService.Instance.GetPaymentInfo(OrderId);
            string message;
            if (!CheckOrder(order, out message)) {
                throw new HttpException(500, message);
            }

            var gateway = GatewayManagement.Instance.Get(GatewayName);
            if (gateway == null) {
                throw new HttpException(500, "payment gateway is not found.");
            }

            var proxy = (IHttpProxy)Activator.CreateInstance(Type.GetType(gateway.PaymentRequestTypeName), order, gateway.Parames);

            proxy.Render(httpContext);          
        }

        bool CheckOrder(IPayment order, out string message)
        {
            if (order == null) {
                message = string.Format("order(no:{0}) is not found.", order.OrderNo);
                return false;
            }

            if (order.Prepaid) {
                message = string.Format("order(no:{0}) was paid.", order.OrderNo);
                return false;
            }
            if (order.Closed) {
                message = string.Format("order(no:{0}) has been closed.", order.OrderNo);
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
            GatewayName = context.Request.QueryString["gateway"];
            OrderId = context.Request.QueryString["orderId"];

            this.ProcessRequest(new HttpContextWrapper(context));

            context.Response.End();
        }
    }
}
