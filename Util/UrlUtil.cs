using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Util
{
    /// <summary>
    /// Url处理
    /// </summary>
    class UrlUtil
    {
        /// <summary>
        /// 修正url格式
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FixUrl(string url)
        {
            //如果url以 / 结尾
            if (url.LastIndexOf("/") == url.Length - 1)
            {
                url = url.Replace("//", "@");
                url = url.Substring(0, url.IndexOf("/"));
                url = url.Replace("@", "//");
            }
            if (url.Contains(":") == false)
            {
                url = $"http://{url}";
            }
            return url;
        }

        public static string GetImageFromUrl(string url)
        {
            string imageName = "";
            if (RegexUtil.IsInvalidImgUrl(url))
            {
                int lastIndex = url.LastIndexOf("/") + 1;
                int lenth = url.Length - lastIndex;
                imageName = url.Substring(lastIndex, lenth);
            }
            return imageName;
        }
    }
}
