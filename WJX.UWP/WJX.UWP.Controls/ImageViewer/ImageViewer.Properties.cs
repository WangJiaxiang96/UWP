using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WJX.UWP.Controls
{
    /// <summary>
    /// ImageViewer的DependencyProperty
    /// </summary>
    public partial class ImageViewer
    {
        /// <summary>
        /// 当前缩放倍数
        /// </summary>
        public float ZoomFactor
        {
            get
            {
                if (_zoom != null)
                    return _zoom.ZoomFactor;
                else
                    return 1;
            }
            set
            {
                if (_zoom != null)
                    _zoom.ChangeView(null, null, value);
            }
        }
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(float), typeof(ImageViewer), new PropertyMetadata(1));

        /// <summary>
        /// 图片相对于整个控件的大小比率，以图片的长或宽（取最大值）为计算基准
        /// </summary>
        public double ImageSizePercent
        {
            get { return (double)GetValue(ImageSizePercentProperty); }
            set { SetValue(ImageSizePercentProperty, value); }
        }
        public static readonly DependencyProperty ImageSizePercentProperty =
            DependencyProperty.Register("ImageSizePercent", typeof(double), typeof(ImageViewer), new PropertyMetadata(0.6));
     
        /// <summary>
        /// 背景透明度
        /// </summary>
        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }
        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(ImageViewer), new PropertyMetadata(0.4));

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        public float MaxScaleRate
        {
            get
            {
                return (float)GetValue(MaxScaleRateProperty);
            }
            set
            {
                SetValue(MaxScaleRateProperty, value);
            }
        }
        public static readonly DependencyProperty MaxScaleRateProperty =
            DependencyProperty.Register("MaxScaleRate", typeof(float), typeof(ImageViewer), new PropertyMetadata(4));

        /// <summary>
        /// 最小缩放倍数
        /// </summary>
        public float MinScaleRate
        {
            get
            {
                return (float)GetValue(MinScaleRateProperty);
            }
            set
            {
                SetValue(MinScaleRateProperty, value);
            }
        }
        public static readonly DependencyProperty MinScaleRateProperty =
            DependencyProperty.Register("MinScaleRate", typeof(float), typeof(ImageViewer), new PropertyMetadata(0.5));

        /// <summary>
        /// 设置可见性
        /// </summary>
        public new Visibility Visibility
        {
            get
            {
                return (Visibility)GetValue(VisibilityProperty);
            }
            set
            {
                if (_zoom != null)
                    _zoom.Visibility = value;
                if (_bg != null)
                    _bg.Visibility = value;
                if (_img != null)
                {
                    AjustImageSize();
                    ResetImagePosition();
                }
                SetValue(VisibilityProperty, value);
            }
        }
        public new static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ImageViewer), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// 图片源
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set
            {
                SetValue(SourceProperty, value);
                //if (_img != null)
                //{
                //    AjustImageSize();
                //    ResetImagePosition();
                //}
            }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null));

    }
}
