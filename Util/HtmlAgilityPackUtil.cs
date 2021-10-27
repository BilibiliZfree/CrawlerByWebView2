using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Util
{
    /// <summary>
    /// 需要引用HtmlAgilityPack包
    /// 使用HtmlAgilityPack包处理html页面数据
    /// </summary>
    class HtmlAgilityPackUtil
    {
        /// <summary>
        /// 从html页面中挑出img标签的src值
        /// </summary>
        /// <param name="html"></param>
        /// <param name="imgTagName"></param>
        /// <returns></returns>
        public static async Task<List<string>> GetImgFromHtmlAsync(string html, string imgTagName = "img")
        {
            Task<List<string>> task = Task.Run(() =>
            {
                //用于存放返回数据
                List<string> list = new List<string>();
                //用于存放筛选出的图片数据
                var imgTagList = GetTagList(html, imgTagName);
                foreach (var image in imgTagList)
                {
                    //逐条数据挑出标签中的src属性值
                    var src = image.Attributes["src"];
                    //判断，值不为空便加入list中
                    if (src == null)
                        continue;
                    list.Add(src.Value);
                }
                return list;
            });
            return await task;
        }
        /// <summary>
        /// 获得html页面中tagName节点数据
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static HtmlAgilityPack.HtmlNodeCollection GetTagList(string html, string tagName)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            try
            {
                //使用HtmlAgilityPack.HtmlDocument解析html数据
                htmlDocument.LoadHtml(html);
                //从HtmlAgilityPack.HtmlDocument数据中筛选出tagName节点中的数据
                return htmlDocument.DocumentNode.SelectNodes("//" + tagName);
            }
            catch
            {
                //无法解析则返回null
                return null;
            }
        }
    }
}
