using System.Web;

namespace ThinkPay
{
    /// <summary>
    /// 与第三方支付平台交易接口的代理
    /// </summary>
    public interface IHttpProxy
    {
        /// <summary>
        /// 渲染http请求
        /// </summary>
        void Render(HttpContextBase httpContext);
    }
}