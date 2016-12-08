using System;
using System.Collections.Generic;
using System.Xml;

namespace ThinkPay
{
    public class GatewayManagement
    {
        /// <summary>
        /// 第三方支付成功后的后台通知页面
        /// </summary>
        public string PaymentNotifyUrl { get; private set; }
        /// <summary>
        /// 第三方支付成功后的前台返回页面
        /// </summary>
        public string PaymentReturnUrl { get; private set; }

        /// <summary>
        /// 第三方退款成功后的后台通知页面
        /// </summary>
        public string RefundNotifyUrl { get; private set; }


        public static readonly GatewayManagement Instance = new GatewayManagement();

        private List<Gateway> gateways;

        private GatewayManagement()
        {
            gateways = new List<Gateway>();
        }

        public Gateway Get(string name)
        {
            return null;
        }

        public Gateway FindByDns(string hostName)
        {
            return gateways.Find(item => Array.FindIndex(item.Keywords.Split(','), key => hostName.Contains(key)) != -1);
        }

        private void Init(string fileName)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);

            var elem = xml.SelectSingleNode("gateway") as XmlElement;
            this.ReturnUrl = elem.GetAttribute("returnUrl");
            this.NotifyUrl = elem.GetAttribute("notifyUrl");

            foreach(XmlNode node in elem.SelectNodes("provider")) {
                elem = node as XmlElement;
                new Gateway() {
                    Display = elem.GetAttribute("displayName"),
                    Enabled = bool.Parse(elem.GetAttribute("enabled")),
                    LogoUrl = elem.GetAttribute("logo"),
                    PaymentNotifyType = Type.GetType(node.SelectSingleNode("properties/").InnerText)
                };
            }
        }
    }
}
