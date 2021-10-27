using Crawler.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Util
{
    public class GlobalDataUtil
    {
        private static readonly object obj = new object();
        private static GlobalDataUtil _instance;

        private ChromiumBrowser browser;
        public ChromiumBrowser Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        public static GlobalDataUtil GetInstance()
        {

            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                        _instance = new GlobalDataUtil();
                }
            }
            return _instance;
        }

        public GlobalDataUtil()
        {
            browser = new ChromiumBrowser();
            browser.Show();
        }
    }
}
