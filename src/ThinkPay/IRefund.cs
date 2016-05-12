
namespace ThinkPay
{
    public interface IRefund
    {
        /// <summary>
        /// 支付网关
        /// </summary>
        string PayGateway { get; }

        /// <summary>
        /// 退款单号
        /// </summary>
        string RefundNo { get; }
        /// <summary>
        /// 第三方支付接口交易号
        /// </summary>
        string TradeNo { get; }
        /// <summary>
        /// 退款金额
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// 被拒绝
        /// </summary>
        bool Rejected { get; }
        /// <summary>
        /// 已完成
        /// </summary>
        bool Completed { get; }
    }
}
