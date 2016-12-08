using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ThinkPay.Utils;



namespace ThinkPay.Alipay
{
    public class DirectpayNotify : ReceiceNotify<PaymentNotifyInfo>
    {
        protected override bool ProcessReceipt(PaymentNotifyInfo reply)
        {
            if (reply.TradeStatus == TradeStatus.Success) {
                var order = BillService.Current.GetPaymentInfo(reply.OriginalOrderNo);
                if(order == null) {
                    Trace.WriteLine(string.Format("The Order of '{0}' is not found.", reply.OriginalOrderNo), "ThinkPay");
                    return false;
                }
                if(order.Prepaid) {
                    Trace.WriteLine(string.Format("The Order of '{0}' was paid.", reply.OriginalOrderNo), "ThinkPay");
                    return true;
                }
                if(order.Amount != reply.TradeAmount) {
                    Trace.WriteLine(string.Format("The Order of '{0}' Amount was paid.", 
                        reply.OriginalOrderNo, reply.TradeAmount, order.Amount), "ThinkPay");
                    return false;
                }
            }

            return BillService.Current.Notify(reply);
        }

        protected override PaymentNotifyInfo Transform(IDictionary parameters)
        {
            PaymentNotifyInfo reply = new PaymentNotifyInfo();
            reply.OriginalOrderNo = parameters["out_trade_no"].ToString();
            reply.TradeNo = parameters["trade_no"].ToString();
            switch (parameters["trade_status"].ToString()) {
                case "TRADE_CLOSED":
                    reply.TradeDate = Convert.ToDateTime(parameters["gmt_close"]);
                    reply.TradeStatus = TradeStatus.Closed;
                    break;
                case "TRADE_SUCCESS":
                case "TRADE_FINISHED":
                    reply.TradeDate = Convert.ToDateTime(parameters["gmt_payment"]);
                    reply.TradeStatus = TradeStatus.Success;
                    break;
                case "WAIT_BUYER_PAY":
                    reply.TradeDate = Convert.ToDateTime(parameters["gmt_create"]);
                    reply.TradeStatus = TradeStatus.Created;
                    break;
            }
            reply.TradeAmount = Convert.ToDecimal(parameters["total_fee"]);

            return reply;
        }

        private bool RemoteSignVerify(string partner, string notifyId)
        {
            if(string.IsNullOrWhiteSpace(notifyId))
                return true;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("service", "notify_verify");
            dict.Add("partner", partner);
            dict.Add("notify_id", notifyId);

            try {
                var reply = HttpUtil.BuildRequestWithGet("https://mapi.alipay.com/gateway.do", dict, 120000);
                return Convert.ToBoolean(reply);
            }
            catch (Exception) {                
                return false;
            }
        }

        protected override bool SignVerify(IDictionary parameters)
        {
            string primaryKey = parameters["key"].ToString();
            string charset = parameters["_input_charset"].ToString();
            string sign = parameters["sign"].ToString();
            string notifyId = parameters["notify_id"].ToString();
            string partner = parameters["partner"].ToString();

            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            for (IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext();) {
                var keyCurrent = key.Current.ToString();
                var valueCurrent = value.Current.ToString();

                if (value == null || string.IsNullOrWhiteSpace(valueCurrent))
                    continue;

                switch (keyCurrent.ToLower()) {
                    case "sign_type":
                    case "sign":
                    case "key":
                        break;
                    default:
                        dict.Add(keyCurrent, valueCurrent);
                        break;
                }               
            }

            return AlipayUtil.CreateSign(dict, primaryKey, charset) == sign && RemoteSignVerify(partner, notifyId);
        }
    }
}
