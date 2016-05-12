using System.Collections;
using System.Collections.Generic;
using System.Web;


namespace ThinkPay
{
    public abstract class ReceiceNotify<T> : IHttpProxy
    {
        /// <summary>
        /// 单据处理
        /// </summary>
        protected abstract bool ReceiptProcessing(T reply);
        /// <summary>
        /// 签名验证
        /// </summary>
        protected abstract bool SignVerify(IDictionary parameters);
        /// <summary>
        /// 通知
        /// </summary>
        protected abstract T NotifyResultConvert(IDictionary parameters);

        /// <summary>
        /// 构造参数
        /// </summary>
        protected virtual IDictionary BuildParameters(HttpRequestBase httpRequest)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (var key in httpRequest.Form.AllKeys) {
                parameters.Add(key, httpRequest.GetFormString(key));
            }

            return parameters;
        }

        public void Render(HttpContextBase httpContext)
        {
            var parameters = BuildParameters(httpContext.Request);

            if (parameters.Count <= 0) {
                httpContext.Response.Write("No notification parameters");
                return;
            }

            if (!SignVerify(parameters)) {
                httpContext.Response.Write("Verify the signature failure");
                return;
            }

            var reply = this.NotifyResultConvert(parameters);
            if (!ReceiptProcessing(reply)) {
                httpContext.Response.Write("fail");
                return;
            }

            httpContext.Response.Write("success");
        }
    }
}
