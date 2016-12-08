
namespace ThinkPay
{
    /// <summary>
    /// 退款单
    /// </summary>
    public class RefundInfo
    {
        /// <summary>
        /// 退款的网关
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 退款单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 第三方支付接口交易号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退款说明
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 是否拒绝(如该退款被关闭)
        /// </summary>
        public bool Rejected { get; set; }
        /// <summary>
        /// 是否退款完成
        /// </summary>
        public bool Completed { get; set; }
    }
}
