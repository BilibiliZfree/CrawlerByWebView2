using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Util
{
    /// <summary>
    /// url判定
    /// </summary>
    class RegexUtil
    {
        /// <summary>
        /// 判断是否是以Http或Https开头的链接
        /// </summary>
        /// <param name="urlStr"></param>
        /// <param name="isStartWithHttp"></param>
        /// <returns></returns>
        public static bool IsUrl(string urlStr, out bool isStartWithHttp)
        {
            bool result = false;
            isStartWithHttp = false;
            //非Http开头的链接
            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlNoHttpPattern))
            {
                result = true;
                isStartWithHttp = false;
            }
            //以Http或Https开头的链接
            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpPattern) ||
                Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpsPattern))
            {
                result = true;
                isStartWithHttp = true;
            }
            return result;
        }

        /// <summary>
        /// 判断是否为链接
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        public static bool IsUrl(string urlStr)
        {
            if (Regex.IsMatch(urlStr, RegexPattern.MatchUrlNoHttpPattern) ||
                Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpPattern) ||
                Regex.IsMatch(urlStr, RegexPattern.MatchUrlWithHttpsPattern))
                return true;
            return false;
        }

        /// <summary>
        /// 判断是否为图片链接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsInvalidImgUrl(string url)
        {
            return RegexMatch(url, RegexPattern.MatchImgPattern).Success;
        }

        public static MatchCollection Matches(string text, string pattern)
        {
            return Regex.Matches(text, pattern);
        }

        /// <summary>
        /// 使用正则表达式pattern匹配text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Match RegexMatch(string text, string pattern)
        {
            //如果传入文本有一个为空，则返回匹配空
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
                return Match.Empty;

            //返回匹配结果
            return Regex.Match(text, pattern);
        }
    }
}
