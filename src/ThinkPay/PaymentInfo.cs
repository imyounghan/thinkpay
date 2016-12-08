using System;

namespace ThinkPay
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class PaymentInfo
    {
        ///// <summary>
        ///// 订单日期
        ///// </summary>
        //public DateTime Date { get; set; }
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
        /// <summary>
        /// 是否已支付
        /// </summary>
        public bool Prepaid { get; set; }
        /// <summary>
        /// 是否关闭(如超时系统自动关闭)
        /// </summary>
        public bool Closed { get; set; }
    }
}
