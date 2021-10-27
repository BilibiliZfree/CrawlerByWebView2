using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Util
{
    class WebUtil
    {
        public static async Task<string> GetHtmlSource(string url, Encoding encoding = null)
        {
            try
            {
                //不受信任的HTTPS
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET"; //默认就是GET
                using (WebResponse response = await request.GetResponseAsync())
                {
                    Encoding tempEncoding = Encoding.Default;

                    if (encoding == null)
                    {
                        tempEncoding = EncodingUtil.GetEncoding(url);
                    }
                    else
                    {
                        tempEncoding = encoding;
                    }

                    Stream stream = response.GetResponseStream();

                    //GZIP流
                    if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }

                    using (StreamReader sr = new StreamReader(stream, tempEncoding))
                    {
                        return sr.ReadToEnd();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="accept"></param>
    /// <param name="userAgent"></param>
    /// <param name="encoding"></param>
    /// <remarks>以后再改</remarks>
    /// <returns></returns>
        public static async Task<string> GetHtmlSource(string url, string accept, string userAgent, Encoding encoding = null, CookieContainer cookieContainer = null)
    {
        try
        {
            //不受信任的HTTPS
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET"; //默认就是GET
            request.Accept = accept;
            request.UserAgent = userAgent;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;

            using (WebResponse response = await request.GetResponseAsync())
            {
                Encoding tempEncoding = Encoding.Default;

                if (encoding == null)
                {
                    tempEncoding = EncodingUtil.GetEncoding(url);
                }
                else
                {
                    tempEncoding = encoding;
                }

                Stream stream = response.GetResponseStream();

                //GZIP流
                if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }

                using (StreamReader sr = new StreamReader(stream, tempEncoding))
                {
                    return sr.ReadToEnd();
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

        public static async Task<Tuple<string, CookieContainer>> GetHtmlSource(string url, CookieContainer cookieContainer, string accept = "", string userAgent = "", Encoding encoding = null)
    {
        //现在很多是共用的，直接复制过来了，以后再统一整理吧。
        try
        {
            //不受信任的HTTPS
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET"; //默认就是GET

            if (!string.IsNullOrEmpty(accept))
                request.Accept = accept;

            if (!string.IsNullOrEmpty(userAgent))
                request.UserAgent = userAgent;

            request.CookieContainer = cookieContainer;

            using (WebResponse response = await request.GetResponseAsync())
            {
                Encoding tempEncoding = Encoding.Default;

                if (encoding == null)
                {
                    tempEncoding = EncodingUtil.GetEncoding(url);
                }
                else
                {
                    tempEncoding = encoding;
                }

                Stream stream = response.GetResponseStream();

                //GZIP流
                if (((HttpWebResponse)response).ContentEncoding.ToLower().Contains("gzip"))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }

                using (StreamReader sr = new StreamReader(stream, tempEncoding))
                {
                    var html = sr.ReadToEnd();
                    CookieCollection responseCookieContainer = ((HttpWebResponse)response).Cookies;
                    stream.Close();
                    return new Tuple<string, CookieContainer>(html, cookieContainer);
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

        public static async Task<Stream> GetHtmlStreamAsync(string url)
    {
        try
        {
            //不受信任的HTTPS
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => { return true; });

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET"; //默认就是GET
            WebResponse response = await request.GetResponseAsync();
            return response.GetResponseStream();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    }
}
