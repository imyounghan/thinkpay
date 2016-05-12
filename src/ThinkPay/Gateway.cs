using System.Collections;

namespace ThinkPay
{
    public class Gateway
    {
        /// <summary>
        /// 支付网关名称
        /// </summary>
        public string Name { get; set; }
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

        public string PaymentRequestTypeName { get; set; }
        public string PaymentNotifyTypeName { get; set; }

        public string RefundRequestTypeName { get; set; }
        public string RefundNotifyTypeName { get; set; }



        public IDictionary Parames { get; set; }
    }
}
