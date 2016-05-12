using System.Web;

namespace ThinkPay
{
    /// <summary>
    /// 支付网关路由器
    /// </summary>
    public interface IGatewayRouter
    {
        /// <summary>
        /// 路由结果
        /// </summary>
        string Route(HttpRequestBase httpRequest);
    }
}
