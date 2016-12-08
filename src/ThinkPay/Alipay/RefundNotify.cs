using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ThinkPay.Utils;

namespace ThinkPay.Alipay
{
    public class RefundNotify : ReceiceNotify<RefundNotifyInfo>
    {
        protected override bool ProcessReceipt(RefundNotifyInfo reply)
        {
            if(!reply.Success) {
                return false;
            }

            var order = BillService.Current.GetRefundInfo(reply.OriginalOrderNo);
            if(order == null) {
                Trace.WriteLine(string.Format("The Refund Order of '{0}' is not found.", reply.OriginalOrderNo), "ThinkPay");
                return false;
            }
            if(order.Completed) {
                Trace.WriteLine(string.Format("The Refund Order of '{0}' was paid.", reply.OriginalOrderNo), "ThinkPay");
                return true;
            }
            if(order.Amount != reply.Amount) {
                Trace.WriteLine(string.Format("The Refund Order of '{0}' Amount was paid.",
                    reply.OriginalOrderNo, reply.Amount, order.Amount), "ThinkPay");
                return false;
            }

            return BillService.Current.Notify(reply);
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
            catch(Exception) {
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
            for(IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
                var keyCurrent = key.Current.ToString();
                var valueCurrent = value.Current.ToString();

                if(value == null || string.IsNullOrWhiteSpace(valueCurrent))
                    continue;

                switch(keyCurrent.ToLower()) {
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

        protected override RefundNotifyInfo Transform(IDictionary parameters)
        {
            RefundNotifyInfo reply = new RefundNotifyInfo();
            reply.OriginalOrderNo = parameters["batch_no"].ToString();
            reply.Timestamp = Convert.ToDateTime(parameters["notify_time"]);

            var results = parameters["result_details"].ToString().Split('^');
            reply.Amount = decimal.Parse(results[1]);
            reply.Success = results[2] == "SUCCESS";

            return reply;
        }
    }
}
