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
    /// ImageViewer控件可在整个Page范围内展示图片，类似“知乎UWP”查看图片细节的控件
    /// </summary>
    public sealed partial class ImageViewerNew : Control
    {
        #region Field

        private Image _img;
        private Grid _bg;
        private ScrollViewer _zoom;
        private TranslateTransform _originPosition;
        private float _zoomfactor;
        #endregion

        #region DependencyProperty

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageViewerNew), new PropertyMetadata(null));

        #endregion

        public ImageViewerNew()
        {
            this.DefaultStyleKey = typeof(ImageViewerNew);
            //OnApplyTemplate();
        }

        protected override void OnApplyTemplate()
        {
            _img = (Image)GetTemplateChild("MainImage");
            _bg = (Grid)GetTemplateChild("BackgroundGrid");
            _zoom = (ScrollViewer)GetTemplateChild("ZoomScrollViewer");

            if (_img != null)
            {
                _img.RenderTransform = _originPosition;

                _img.Tapped += _img_Tapped;
                _img.RightTapped += _img_RightTapped;
                _img.SizeChanged += _img_SizeChanged;
                _img.DoubleTapped += _img_DoubleTapped;
            }
            if (_zoom != null)
            {
                _zoom.Tapped += _zoom_Tapped;
                _zoom.ManipulationDelta += _zoom_ManipulationDelta;
                _zoom.PointerWheelChanged += _zoom_PointerWheelChanged;
                _zoom.DirectManipulationStarted += _zoom_DirectManipulationStarted;
                _zoom.ManipulationStarted += _zoom_ManipulationStarted;
                _zoom.ViewChanged += _zoom_ViewChanged;
            }

            _originPosition = new TranslateTransform();
            _zoomfactor = 1;

            base.OnApplyTemplate();
        }

        private void _zoom_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            _zoom_PointerWheelChanged(null, null);
        }

        private void _zoom_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
        }

        private void _zoom_DirectManipulationStarted(object sender, object e)
        {
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 调整图片显示大小为当前控价大小的60%（以图片长宽中数值最大的数为基准计算）
        /// </summary>
        private void AjustImageSize()
        {
            double height = _img.ActualHeight;
            double width = _img.ActualWidth;
            if (height >= width)
            {
                _img.Height = 0.6 * this.ActualHeight;
            }
            else
            {
                _img.Width = 0.6 * this.ActualWidth;
            }

        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;

            if (_zoom != null)
            {
                //将图片缩放倍数还原
                _zoom.ChangeView(null, null, 1);

                RecoverImagePosition();
            }
        }

        /// <summary>
        /// 将图片位置还原
        /// </summary>
        private void RecoverImagePosition()
        {
            if (_img != null)
            {
                _originPosition.X = 0;
                _originPosition.Y = 0;

                _zoom.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
                _isFromOrigin1 = true;
                _isFromOrigin2 = true;
            }
        }
    }
}

