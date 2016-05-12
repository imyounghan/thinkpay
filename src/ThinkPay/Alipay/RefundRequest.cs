using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ThinkPay.Alipay
{
    /// <summary>
    /// 退款请求
    /// </summary>
    public class RefundRequest : AlipayRequest
    {
        private readonly IDictionary _formData;
        public RefundRequest(IRefund refund, IDictionary parameters)
        {
            _formData = BuildFormData(parameters);
        }

        protected override IDictionary FormData
        {
            get
            {
                return _formData;
            }
        }

        protected virtual IDictionary<string, string> PaymentConvert(IRefund refund)
        {
            //var data = payment as PaymentInfo;
            //if (data == null)
            //    throw new Exception("");

            Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("subject", data.Subject);
            //if (!string.IsNullOrWhiteSpace(data.Body)) {
            //    dict.Add("body", data.Body);
            //}
            //dict.Add("out_trade_no", data.OrderNo);
            //dict.Add("total_fee", data.Amount.ToString("F", CultureInfo.InvariantCulture));
            //if (!string.IsNullOrWhiteSpace(data.ShowUrl)) {
            //    dict.Add("show_url", data.ShowUrl);
            //}

            return dict;
        }

        private static IDictionary BuildFormData(IDictionary parameters)
        {
            string primaryKey = parameters["key"].ToString();
            string charset = parameters["_input_charset"].ToString();

            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            for (IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
                var keyCurrent = key.Current.ToString();
                var valueCurrent = value.Current.ToString();

                if (string.IsNullOrWhiteSpace(valueCurrent) || new string[] { "key", "sign", "sign_type" }.Contains(keyCurrent, StringComparer.CurrentCultureIgnoreCase))
                    continue;

                dict.Add(keyCurrent, valueCurrent);
            }
            dict.Add("service", "refund_fastpay_by_platform_pwd");
            //dict.Add("refund_date", "");
            //dict.Add("batch_no", "");
            //dict.Add("batch_num", "1");
            //dict.Add("detail_data", "");

            var final = new Dictionary<string, string>(dict);
            final.Add("sign", BuildSign(dict, primaryKey, charset));
            final.Add("sign_type", "MD5");

            return final;
        }
    }
}
