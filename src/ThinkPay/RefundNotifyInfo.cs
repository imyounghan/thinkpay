using System;

namespace ThinkPay
{
    public class RefundNotifyInfo
    {
        /// <summary>
        /// 商家的退款单号
        /// </summary>
        public string OriginalOrderNo { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 通知时间
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 是否退款成功
        /// </summary>
        public bool Success { get; set; }
    }
}
