﻿using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace ThinkPay.Alipay
{
    public class DirectpayRequest : AlipayRequest
    {
        private readonly IDictionary _formInputs;
        public DirectpayRequest(PaymentInfo payment, IDictionary parameters)
        {
            foreach (KeyValuePair<string, string> temp in PaymentConvert(payment)) {
                parameters.Add(temp.Key, temp.Value);
            }
            _formInputs = BuildFormData(parameters, TradeMode.Pay);
        }

        private IDictionary<string, string> PaymentConvert(PaymentInfo payment)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("subject", payment.Subject);
            if(!string.IsNullOrWhiteSpace(payment.Body)) {
                dict.Add("body", payment.Body);
            }
            dict.Add("out_trade_no", payment.OrderNo);
            dict.Add("total_fee", payment.Amount.ToString("F", CultureInfo.InvariantCulture));
            if(!string.IsNullOrWhiteSpace(payment.ShowUrl)) {
                dict.Add("show_url", payment.ShowUrl);
            }

            return dict;
        }

        protected override IDictionary FormInputs { get { return _formInputs; } }
        
       
        //private IDictionary BuildFormData(IDictionary parameters)
        //{
        //    string primaryKey = parameters["key"].ToString();
        //    string charset = parameters["_input_charset"].ToString();

        //    string[] excludeArray = new string[] { "key", "sign", "sign_type" };
        //    SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
        //    for (IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
        //        var keyCurrent = key.Current.ToString();
        //        var valueCurrent = value.Current.ToString();

        //        if (string.IsNullOrWhiteSpace(valueCurrent) || excludeArray.Contains(keyCurrent, StringComparer.CurrentCultureIgnoreCase))
        //            continue;

        //        dict.Add(keyCurrent, valueCurrent);
        //    }
        //    dict.Add("service", "create_direct_pay_by_user");

        //    var final = new Dictionary<string, string>(dict);
        //    final.Add("sign", BuildSign(dict, primaryKey, charset));
        //    final.Add("sign_type", "MD5");

        //    return final;
        //}
    }
}
