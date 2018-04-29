using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace WJX.UWP.Controls
{
	/// <summary>
    /// ImageViewer的DependencyProperty
    /// </summary>
    public partial class ImageViewer
    {
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
        /// 图片源
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageViewer), new PropertyMetadata(null));

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
        public double MaxScaleRate
        {
            get
            {
                if (_scale != null)
                    return _scale.MaxScaleRate;
                else
                    return (double)GetValue(MaxScaleRateProperty);
            }
            set
            {
                if (_scale != null)
                    _scale.MaxScaleRate = value;
                SetValue(MaxScaleRateProperty, value);
            }
        }
        public static readonly DependencyProperty MaxScaleRateProperty =
            DependencyProperty.Register("MaxScaleRate", typeof(double), typeof(ImageViewer), new PropertyMetadata(4));

        /// <summary>
        /// 最小缩放倍数
        /// </summary>
        public double MinScaleRate
        {
            get
            {
                if (_scale != null)
                    return _scale.MinScaleRate;
                else
                    return (double)GetValue(MinScaleRateProperty);
            }
            set
            {
                if (_scale != null)
                    _scale.MinScaleRate = value;
                SetValue(MinScaleRateProperty, value);
            }
        }
        public static readonly DependencyProperty MinScaleRateProperty =
            DependencyProperty.Register("MinScaleRate", typeof(double), typeof(ImageViewer), new PropertyMetadata(0.5));

        /// <summary>
        /// 设置可见性
        /// </summary>
        public new Visibility Visibility
        {
            get
            {
                if (_scale != null)
                    return _scale.Visibility;
                else
                    return (Visibility)GetValue(VisibilityProperty);
            }
            set
            {
                if (_scale != null)
                    _scale.Visibility = value;
                SetValue(VisibilityProperty, value);
            }
        }
        public new static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ImageViewer), new PropertyMetadata(Visibility.Collapsed));

    }
}
