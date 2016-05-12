
namespace ThinkPay
{
    /// <summary>
    /// 单据服务接口
    /// </summary>
    public interface IBillService
    {
        /// <summary>
        /// 获取支付单信息
        /// </summary>
        IPayment GetPaymentInfo(string paymentNo);
        /// <summary>
        /// 获取退款单信息
        /// </summary>
        IRefund GetRefundInfo(string refundNo);

        /// <summary>
        /// 支付完成通知处理
        /// </summary>
        bool PaymentNofity(IPaymentNotify paymentReply);
        /// <summary>
        /// 退款成功通知处理
        /// </summary>
        bool RefundNotify(IRefundNotify refundReply);
    }
}
