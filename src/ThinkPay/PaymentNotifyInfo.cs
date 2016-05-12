using System;

namespace ThinkPay
{
    public class PaymentNotifyInfo : IPaymentNotify
    {
        public PaymentNotifyInfo(string gateway)
        {
            this.PayGateway = gateway;
        }

        public string PayGateway { get; set; }

        public DateTime Date { get; set; }

        public string OrderNo { get; set; }

        public string TradeNo { get; set; }

        public decimal Amount { get; set; }

        public TradeStatus TradeStatus { get; set; }
    }
}
