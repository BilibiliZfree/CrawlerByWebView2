using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Util
{
    /// <summary>
    /// 常量字符串匹配类
    /// </summary>
    class RegexPattern
    {
        //匹配网址 RegexOptions.Multiline
        public const string MatchUrlWithHttpPattern = "^http://\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov|net)([/\\S]+)$";
        public const string MatchUrlWithHttpsPattern = "^https://\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov|net)([/\\S]+)$";
        public const string MatchUrlNoHttpPattern = "^\\S+\\.(com|cn|top|vip|ltd|shop|wang|club|online|store|site|tech|fun|biz|info|com.cn|org|org.cn|gov.cn|gov|net)([/\\S]+)$";
        public const string MatchFileUrlWithHttpPattern = "http\\S*\\.(jpg|png|bmp|mp4|exe|rar|zip)";
        public const string MatchFileUrlWithForwardSlash = "/\\S*\\.(jpg|png|bmp|mp4|exe|rar|zip)";

        //匹配有效图像路径
        public const string MatchImgPattern = "(ftp|http|https)://(\\S*/)+\\S*.(png|jpg|gif|jiff|jpeg|bmp)";
        //匹配有效的<img/>标签
        public const string TagImgPattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<image>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";

        //charset
        public const string CharsetPattern = @"<meta[\s\S]+?charset=(?<charset>(.*?))""[\s\S]+?>";
    }
}
