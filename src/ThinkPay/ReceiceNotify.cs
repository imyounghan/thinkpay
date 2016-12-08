using System.Collections;
using System.Collections.Generic;
using System.Web;


namespace ThinkPay
{
    /// <summary>
    /// 收到通知
    /// </summary>
    public abstract class ReceiceNotify<T> : IHttpProxy
    {
        //public string PaymentFrom { get; set; }

        /// <summary>
        /// 处理单据
        /// </summary>
        protected abstract bool ProcessReceipt(T reply);
        /// <summary>
        /// 签名验证
        /// </summary>
        protected abstract bool SignVerify(IDictionary parameters);
        /// <summary>
        /// 将第三方结果转换成 <see cref="T"/>
        /// </summary>
        protected abstract T Transform(IDictionary parameters);

        /// <summary>
        /// 构造参数
        /// </summary>
        protected virtual IDictionary BuildParameters(HttpRequestBase httpRequest)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (var key in httpRequest.Form.AllKeys) {
                parameters.Add(key, httpRequest.Form[key]);
            }

            return parameters;
        }

        public void Render(HttpContextBase httpContext)
        {
            var parameters = BuildParameters(httpContext.Request);

            if (parameters.Count <= 0) {
                httpContext.Response.Write("No notification parameters.");
                return;
            }

            if (!SignVerify(parameters)) {
                httpContext.Response.Write("Verify the signature failure.");
                return;
            }

            var reply = this.Transform(parameters);
            if(!ProcessReceipt(reply)) {
                httpContext.Response.Write("fail");
                return;
            }

            httpContext.Response.Write("success");
        }
    }
}
