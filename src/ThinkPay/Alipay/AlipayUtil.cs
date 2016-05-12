using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ThinkPay.Utils;


namespace ThinkPay.Alipay
{
    public static class AlipayUtil
    {
        public static string CreateSign(IDictionary<string, string> parameters, string key, string charset)
        {
            string[] array = parameters.Select(item => string.Format("{0}={1}", item.Key, item.Value)).ToArray();
            string originalstr = string.Concat(string.Join("&", array), key);

            return CryptoUtil.MD5(originalstr, charset);
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        public static string QueryTimestamp(string partner, string charset)
        {
            string url = string.Format("https://mapi.alipay.com/gateway.do?service=query_timestamp&partner={0}&_input_charset={1}", partner, charset);

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

           string encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }
    }
}
