using System;

namespace ThinkPay
{
    public interface IPaymentNotify
    {
        string PayGateway { get; }

        /// <summary>
        /// 交易日期
        /// </summary>
        DateTime Date { get; }
        /// <summary>
        /// 原订单ID
        /// </summary>
        string OrderNo { get; }
        /// <summary>
        /// 第三方支付接口交易号
        /// </summary>
        string TradeNo { get; }
        /// <summary>
        /// 交易金额
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// 交易状态
        /// </summary>
        TradeStatus TradeStatus { get; }
    }
}
