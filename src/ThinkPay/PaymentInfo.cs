using System;

namespace ThinkPay
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class PaymentInfo : IPayment
    {
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单页面展示地址
        /// </summary>
        public string ShowUrl { get; set; }
        /// <summary>
        /// 订单标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 订单的具体描述信息
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }

        public bool Prepaid { get; set; }

        public bool Closed { get; set; }
    }
}
