using System;

namespace ThinkPay
{
    /// <summary>
    /// 支付通知
    /// </summary>
    public class PaymentNotifyInfo
    {
        /// <summary>
        /// 支付平台
        /// </summary>
        public string From { get; set; }

        
        /// <summary>
        /// 商家的订单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 第三方支付平台的交易单号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TradeDate { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public TradeStatus TradeStatus { get; set; }
    }
}
