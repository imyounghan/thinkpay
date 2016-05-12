using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ThinkPay
{
    /// <summary>
    /// 对 基础类型 的扩展
    /// </summary>
    internal static class Extentions
    {
        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="request">一个包含客户端在 Web 请求中发送的 HTTP 值的对象。</param>
        /// <param name="name">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(this HttpRequestBase request, string name)
        {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            if (request.Form[name] == null) {
                return string.Empty;
            }
            return request.Form[name];
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="request">一个包含客户端在 Web 请求中发送的 HTTP 值的对象。</param>
        /// <param name="name">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(this HttpRequestBase request, string name)
        {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            if (request.QueryString[name] == null) {
                return string.Empty;
            }
            return request.QueryString[name];
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        public static string UrlEncode(this string str, string charset = "utf-8")
        {
            return str.UrlEncode(Encoding.GetEncoding(charset));
        }
        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        public static string UrlEncode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlEncode(str, encoding);
        }

        /// <summary>
        /// 返回 URL 字符串的解码结果
        /// </summary>
        public static string UrlDecode(this string str, string charset = "utf-8")
        {
            return str.UrlDecode(Encoding.GetEncoding(charset));
        }
        /// <summary>
        /// 返回 URL 字符串的解码结果
        /// </summary>
        public static string UrlDecode(this string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }



        /// <summary>
        /// 判定字符串是不是数值型
        /// </summary>
        public static bool IsNumeric(this string str)
        {
            return Regex.IsMatch(str, @"^[-]?[0-9]*$");
        }

        /// <summary>
        /// 判断字符串是不是yyyy-mm-dd字符串
        /// </summary>
        public static bool IsDate(this string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        /// <summary>
        /// 判断字符串是不是时间格式
        /// </summary>
        public static bool IsTime(this string str)
        {
            return Regex.IsMatch(str, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        /// 判断字符串是不是日期模式
        /// </summary>
        public static bool IsDateTime(this string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2}) ^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        /// 判断字符串是不是小数类型
        /// </summary>
        public static bool IsDecimal(this string str)
        {
            return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$");
        }

        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 判断是否为IPv4地址
        /// </summary>
        public static bool IsIPV4(this string ip)
        {
            string num = "(25[0-5]|2[0-4]//d|[0-1]//d{2}|[1-9]?//d)";
            return Regex.IsMatch(ip, string.Concat("^", num, "//.", num, "//.", num, "//.", num, "$"));
        }

        /// <summary>
        /// 转换为bool型
        /// </summary>
        public static bool ToBoolean(this string str)
        {
            return ToBoolean(str, false);
        }

        /// <summary>
        /// 转换为bool型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="value">默认值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBoolean(this string str, bool value)
        {
            bool val;
            if (!string.IsNullOrEmpty(str) && bool.TryParse(str, out val)) {
                return val;
            }
            return value;
        }

        /// <summary>
        /// 转换为Int32类型
        /// </summary>
        public static int ToInt(this string str)
        {
            return ToInt(str, 0);
        }

        /// <summary>
        /// 转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="value">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ToInt(this string str, int value)
        {
            int val;
            if (!string.IsNullOrEmpty(str) && int.TryParse(str, out val)) {
                return val;
            }
            return value;
        }


        /// <summary>
        /// 转换为decimal型
        /// </summary>
        public static decimal ToDecimal(this string str)
        {
            return ToDecimal(str, 0);
        }

        /// <summary>
        /// 转换为decimal型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="value">缺省值</param>
        public static decimal ToDecimal(this string str, decimal value)
        {
            decimal val;
            if (!string.IsNullOrEmpty(str) && decimal.TryParse(str, out val)) {
                return val;
            }
            return value;
        }


        /// <summary>
        /// 转换为DateTime型
        /// </summary>
        public static DateTime ToDate(this string str)
        {
            return ToDate(str, DateTime.Today);
        }

        /// <summary>
        /// 转换为DateTime型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="value">缺省值</param>
        public static DateTime ToDate(this string str, DateTime value)
        {
            DateTime date;
            if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out date)) {
                return date;
            }
            return value;
        }
    }
}
