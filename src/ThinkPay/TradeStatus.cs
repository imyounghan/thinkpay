
namespace ThinkPay
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum TradeStatus
    {
        ///// <summary>
        ///// 交易完成
        ///// </summary>
        //Finished,
        /// <summary>
        /// 交易成功
        /// </summary>
        Success,
        /// <summary>
        /// 交易关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 交易创建(是指用户在第三方支付平台创建了交易但还未付款)
        /// </summary>
        Created
    }
}
