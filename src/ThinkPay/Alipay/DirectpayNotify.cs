using System;
using System.Collections;
using System.Collections.Generic;
using ThinkPay.Utils;



namespace ThinkPay.Alipay
{
    public class DirectpayNotify : ReceiceNotify<IPaymentNotify>
    {        
        protected override bool ReceiptProcessing(IPaymentNotify reply)
        {
            if (reply.TradeStatus == TradeStatus.Success) {
                var order = BillService.Instance.GetPaymentInfo(reply.OrderNo);
                if (order.Prepaid)
                    return true;
                if (order.Amount != reply.Amount)
                    return false;
            }

            return BillService.Instance.PaymentNofity(reply);
        }

        protected override IPaymentNotify NotifyResultConvert(IDictionary parameters)
        {
            PaymentNotifyInfo reply = new PaymentNotifyInfo("alipay");
            reply.OrderNo = parameters["out_trade_no"].ToString();
            reply.TradeNo = parameters["trade_no"].ToString();
            switch (parameters["trade_status"].ToString()) {
                case "TRADE_CLOSED":
                    reply.Date = parameters["gmt_close"].ToString().ToDate();
                    reply.TradeStatus = TradeStatus.Closed;
                    break;
                case "TRADE_SUCCESS":
                case "TRADE_FINISHED":
                    reply.Date = parameters["gmt_payment"].ToString().ToDate();
                    reply.TradeStatus = TradeStatus.Success;
                    break;
                case "WAIT_BUYER_PAY":
                    reply.Date = parameters["gmt_create"].ToString().ToDate();
                    reply.TradeStatus = TradeStatus.Created;
                    break;
            }
            reply.Amount = parameters["total_fee"].ToString().ToDecimal();

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
                return HttpUtil.BuildRequestWithGet("https://mapi.alipay.com/gateway.do", dict, 120000).ToBoolean();
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
