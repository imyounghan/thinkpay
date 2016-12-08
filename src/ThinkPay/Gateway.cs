using System;
using System.Collections;

namespace ThinkPay
{
    /// <summary>
    /// 表示支付网关的信息
    /// </summary>
    public class Gateway
    {
        /// <summary>
        /// 支付网关名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Display { get; set; }
        /// <summary>
        /// 图标URL
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 支付请求处理类型
        /// </summary>
        public Type PaymentRequestType { get; set; }
        /// <summary>
        /// 支付回复处理类型
        /// </summary>
        public Type PaymentNotifyType { get; set; }
        /// <summary>
        /// 退款请求处理类型
        /// </summary>
        public Type RefundRequestType { get; set; }
        /// <summary>
        /// 退款通知处理
        /// </summary>
        public Type RefundNotifyType { get; set; }


        /// <summary>
        /// 用于与第三方支付平台对接的参数
        /// </summary>
        public IDictionary Parameters { get; set; }
    }
}
