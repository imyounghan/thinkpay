using System.Collections;
using System.Text;
using System.Web;

namespace ThinkPay
{
    public abstract class SendRequest : IHttpProxy
    {
        /// <summary>
        /// 支付网关URL
        /// </summary>
        protected abstract string Gateway { get; }
        /// <summary>
        /// 表单名称
        /// </summary>
        protected virtual string FormName { get { return "payform"; } }
        /// <summary>
        /// 请求类型
        /// </summary>
        protected virtual string Method { get { return "post"; } }
        /// <summary>
        /// 表单数据
        /// </summary>
        protected abstract IDictionary FormData { get; }


        public virtual void Render(HttpContextBase httpContext)
        {
            StringBuilder html = new StringBuilder();

            html.AppendFormat("<form id=\"{0}\" name=\"{0}\" action=\"{1}\" method=\"{2}\">", FormName, Gateway, Method).AppendLine();
            for (IEnumerator key = FormData.Keys.GetEnumerator(), value = FormData.Values.GetEnumerator(); key.MoveNext() && value.MoveNext(); ) {
                html.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" />",
                    key.Current, value.Current).AppendLine();
            }
            //foreach (KeyValuePair<string, string> temp in FormData) {
            //    html.AppendFormat("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" />", temp.Key, temp.Value).AppendLine();
            //}
            html.Append("<input type=\"submit\" style=\"display:none;\" />").AppendLine().Append("</form>").AppendLine();
            html.AppendFormat("<script>document.forms['{0}'].submit();</script>", FormName);

            httpContext.Response.Write(html.ToString());
        }

        void IHttpProxy.Render(HttpContextBase httpContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
