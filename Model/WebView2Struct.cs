using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Crawler.Model
{
    public class WebView2Struct
    {
        private bool _CanGoback;

        public bool CanGoback
        {
            get { return _CanGoback; }
            set { _CanGoback = value; }
        }

        private bool _CanGoForward;

        public bool CanGoForward
        {
            get { return _CanGoForward; }
            set { _CanGoForward = value; }
        }

        private Color _DefaultBackgroundColor;

        public Color DefaultBackgroundColor
        {
            get { return _DefaultBackgroundColor; }
            set { _DefaultBackgroundColor = value; }
        }

        private Color _DesignModeForegroundColor;

        public Color DesignModeForegroundColor
        {
            get { return _DesignModeForegroundColor; }
            set { _DesignModeForegroundColor = value; }
        }

        private bool _IsInDesignMode;

        public bool IsInDesignMode
        {
            get { return _IsInDesignMode; }
            set { _IsInDesignMode = value; }
        }

        private Uri _Source;

        public Uri Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private double _ZoomFactor;

        public double ZoomFactor
        {
            get { return _ZoomFactor; }
            set { _ZoomFactor = value; }
        }

    }
}
