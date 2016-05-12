using System;

namespace ThinkPay
{
    ///// <summary>
    ///// 订单状态
    ///// </summary>
    //public enum OrderStatus
    //{
    //    /// <summary>
    //    /// 关闭
    //    /// </summary>
    //    Closed,
    //    /// <summary>
    //    /// 正常状态
    //    /// </summary>
    //    Created,
    //    /// <summary>
    //    /// 等待付款
    //    /// </summary>
    //    Waitpay,
    //    /// <summary>
    //    /// 支付
    //    /// </summary>
    //    Paid,
    //    /// <summary>
    //    /// 打包配货
    //    /// </summary>
    //    Packing,
    //    /// <summary>
    //    /// 发货
    //    /// </summary>
    //    Delivered,
    //    /// <summary>
    //    /// 收货
    //    /// </summary>
    //    Received,
    //    /// <summary>
    //    /// 完成
    //    /// </summary>
    //    Finished = 1,
    //}

    /// <summary>
    /// 支付单据
    /// </summary>
    public interface IPayment
    {
        /// <summary>
        /// 订单id
        /// </summary>
        string OrderNo { get; }
        /// <summary>
        /// 订单金额
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// 是否已支付。
        /// </summary>
        bool Prepaid { get; }
        /// <summary>
        /// 是否关闭。
        /// </summary>
        bool Closed { get; }        
    }    
}
