using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ThinkPay.Alipay
{
    public class DirectpayRequest : AlipayRequest
    {
        private readonly IDictionary _formData;
        public DirectpayRequest(IPayment payment, IDictionary parameters)
        {
            foreach (KeyValuePair<string, string> temp in PaymentConvert(payment)) {
                parameters.Add(temp.Key, temp.Value);
            }
            _formData = BuildFormData(parameters);
        }

        protected virtual IDictionary<string, string> PaymentConvert(IPayment payment)
        {
            var data = payment as PaymentInfo;
            if (data == null)
                throw new Exception("");

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("subject", data.Subject);
            if (!string.IsNullOrWhiteSpace(data.Body)) {
                dict.Add("body", data.Body);
            }
            dict.Add("out_trade_no", data.OrderNo);
            dict.Add("total_fee", data.Amount.ToString("F", CultureInfo.InvariantCulture));
            if (!string.IsNullOrWhiteSpace(data.ShowUrl)) {
                dict.Add("show_url", data.ShowUrl);
            }

            return dict;
        }

        protected override IDictionary FormData
        {
            get
            {
                return _formData;
            }
        }

        protected virtual string[] FilterParameters
        {
            get { return new string[] { "key", "sign", "sign_type" }; }
        }
        
        private IDictionary BuildFormData(IDictionary parameters)
        {
            string primaryKey = parameters["key"].ToString();
            string charset = parameters["_input_charset"].ToString();

            string[] excludeArray = this.FilterParameters;
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
            for (IEnumerator key = parameters.Keys.GetEnumerator(), value = parameters.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
                var keyCurrent = key.Current.ToString();
                var valueCurrent = value.Current.ToString();

                if (string.IsNullOrWhiteSpace(valueCurrent) || excludeArray.Contains(keyCurrent, StringComparer.CurrentCultureIgnoreCase))
                    continue;

                dict.Add(keyCurrent, valueCurrent);
            }
            dict.Add("service", "create_direct_pay_by_user");

            var final = new Dictionary<string, string>(dict);
            final.Add("sign", BuildSign(dict, primaryKey, charset));
            final.Add("sign_type", "MD5");

            return final;
        }
    }
}
