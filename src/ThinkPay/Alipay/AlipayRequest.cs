using System.Collections.Generic;
using System.Linq;


namespace ThinkPay.Alipay
{
    public abstract class AlipayRequest : SendRequest
    {
        protected override string FormName
        {
            get
            {
                return "alipayform";
            }
        }

        protected override string Gateway
        {
            get { return "https://mapi.alipay.com/gateway.do"; }
        }

        protected override string Method
        {
            get
            {
                return "get";
            }
        }

        protected static string BuildSign(IDictionary<string, string> parameters, string key, string charset)
        {
            return AlipayUtil.CreateSign(parameters, key, charset);
        }
    }
}
