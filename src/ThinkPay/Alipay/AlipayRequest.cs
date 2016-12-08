using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ThinkPay.Alipay
{
    public abstract class AlipayRequest : SendRequest
    {
        protected override string FormName
        {
            get { return "alipayform"; }
        }

        protected override string Gateway
        {
            get { return "https://mapi.alipay.com/gateway.do"; }
        }

        protected override string FormMethod
        {
            get { return "get"; }
        }

        protected static IDictionary BuildFormData(IDictionary parameters, TradeMode tradeMode)
        {
            string primaryKey = parameters["key"].ToString();
            string charset = parameters["_input_charset"].ToString();

            string[] excludeArray = new string[] { "key", "sign", "sign_type" };
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();

            if(tradeMode == TradeMode.Pay) {
                dict.Add("notify_url", GatewayManagement.Instance.PaymentNotifyUrl);
                dict.Add("return_url", GatewayManagement.Instance.PaymentReturnUrl);
                dict.Add("service", "create_direct_pay_by_user");
            }
            else if(tradeMode == TradeMode.Refund) {
                dict.Add("notify_url", GatewayManagement.Instance.RefundNotifyUrl);
                dict.Add("service", "refund_fastpay_by_platform_pwd");
            }
            
            for(IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
                if(key.Current == null || value.Current == null)
                    continue;

                var keyCurrent = key.Current.ToString();
                var valueCurrent = value.Current.ToString();

                if(string.IsNullOrWhiteSpace(keyCurrent) || string.IsNullOrWhiteSpace(valueCurrent) ||
                    excludeArray.Contains(keyCurrent, StringComparer.CurrentCultureIgnoreCase))
                    continue;

                dict.Add(keyCurrent, valueCurrent);
            }            

            var final = new Dictionary<string, string>(dict);
            final.Add("sign", AlipayUtil.CreateSign(dict, primaryKey, charset));
            final.Add("sign_type", "MD5");

            return final;
        }
    }
}
