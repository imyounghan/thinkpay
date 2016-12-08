using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ThinkPay.Alipay
{
    /// <summary>
    /// 退款请求
    /// </summary>
    public class RefundRequest : AlipayRequest
    {
        private readonly IDictionary _formInputs;
        public RefundRequest(RefundInfo refund, IDictionary parameters)
        {
            foreach(KeyValuePair<string, string> temp in RefundConvert(refund)) {
                parameters.Add(temp.Key, temp.Value);
            }
            _formInputs = BuildFormData(parameters, TradeMode.Refund);
        }

        protected override IDictionary FormInputs
        {
            get { return _formInputs; }
        }
        
        protected virtual IDictionary<string, string> RefundConvert(RefundInfo refund)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("refund_date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dict.Add("batch_no", refund.OrderNo);
            dict.Add("batch_num", "1");
            dict.Add("detail_data", string.Concat(refund.TradeNo, "^", refund.Amount.ToString("F", CultureInfo.InvariantCulture), "^", refund.Comments));

            return dict;
        }

        //private static IDictionary BuildFormData(IDictionary parameters)
        //{
        //    string primaryKey = parameters["key"].ToString();
        //    string charset = parameters["_input_charset"].ToString();

        //    string[] excludeArray = new string[] { "key", "sign", "sign_type" };
        //    SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
        //    for (IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
        //        var keyCurrent = key.Current.ToString();
        //        var valueCurrent = value.Current.ToString();

        //        if(string.IsNullOrWhiteSpace(valueCurrent) || excludeArray.Contains(keyCurrent, StringComparer.CurrentCultureIgnoreCase))
        //            continue;

        //        dict.Add(keyCurrent, valueCurrent);
        //    }
        //    dict.Add("service", "refund_fastpay_by_platform_pwd");

        //    var final = new Dictionary<string, string>(dict);
        //    final.Add("sign", BuildSign(dict, primaryKey, charset));
        //    final.Add("sign_type", "MD5");

        //    return final;
        //}
    }
}
