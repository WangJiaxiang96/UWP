using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
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
    public sealed partial class ImageViewer : Control
    {
        #region Field

        private Image _img;
        private Grid _bg;
        private ScrollViewer _zoom;
        private TranslateTransform _translateTransform;
        //private ScaleTransform _scaleTransform;
        private TransformGroup _transformGroup;
        private float _zoomfactor;
        #endregion

        public ImageViewer()
        {
            this.DefaultStyleKey = typeof(ImageViewer);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_img != null)
            {
                _img.Tapped -= _img_Tapped;
                _img.RightTapped -= _img_RightTapped;
                _img.SizeChanged -= _img_SizeChanged;
                _img.DoubleTapped -= _img_DoubleTapped;
            }
            if (_zoom != null)
            {
                _zoom.Tapped -= _zoom_Tapped;
                _zoom.ManipulationDelta -= _zoom_ManipulationDelta;
                _zoom.ViewChanged -= _zoom_ViewChanged;
            }

            _img = (Image)GetTemplateChild("MainImage");
            _bg = (Grid)GetTemplateChild("BackgroundGrid");
            _zoom = (ScrollViewer)GetTemplateChild("ZoomScrollViewer");

            //初始化_transformGroup
            _translateTransform = new TranslateTransform();
            //_scaleTransform = new ScaleTransform();
            _transformGroup = new TransformGroup();
            //_transformGroup.Children.Add(_scaleTransform);
            _transformGroup.Children.Add(_translateTransform);

            if (_img != null)
            {
                _img.RenderTransform = _transformGroup;

                _img.Tapped += _img_Tapped;
                _img.RightTapped += _img_RightTapped;
                _img.SizeChanged += _img_SizeChanged;
                _img.DoubleTapped += _img_DoubleTapped;
            }
            if (_zoom != null)
            {
                _zoom.Tapped += _zoom_Tapped;
                _zoom.ManipulationDelta += _zoom_ManipulationDelta;
                _zoom.ViewChanged += _zoom_ViewChanged;
            }

            _zoomfactor = 1f;
            this.Visibility = Visibility.Collapsed;
                        
            this.SizeChanged += ImageViewer_SizeChanged;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_img != null)
            {
                AjustImageSize();
                ResetImagePosition();
            }

            return base.MeasureOverride(availableSize);
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            ResetZoomFactor();
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 调整图片显示大小为当前控价大小的百分比（以ImageSizePercent属性的小数表示，以图片长宽中数值最大的数为基准计算）
        /// </summary>
        private void AjustImageSize()
        {
            double height = _img.ActualHeight;
            double width = _img.ActualWidth;

            // 如果宽度小于560，则将图片调整为适应屏幕
            double imageSizeFactor = this.ActualWidth >= 560 ? this.ImageSizePercent : 1;

            if (height >= width)
            {
                _img.Height = imageSizeFactor * this.ActualHeight;
                _img.Width = _img.Height * width / height;
            }
            else
            {
                _img.Width = imageSizeFactor * this.ActualWidth;
                _img.Height = _img.Width * height  / width;
            }
        }

        /// <summary>
        /// 重置缩放倍率
        /// </summary>
        private void ResetZoomFactor()
        {
            if (_zoom != null)
            {
                ResetImagePosition();

                //将图片缩放倍数还原
                _zoom.ChangeView(null, null, 1f);
            }
        }

        /// <summary>
        /// 将图片位置还原
        /// </summary>
        private void ResetImagePosition()
        {
            if (_img != null)
            {
                _translateTransform.X = 0;
                _translateTransform.Y = 0;

                //_scaleTransform.CenterX = this.ActualWidth / 2;
                //_scaleTransform.CenterY = this.ActualHeight / 2;

                _zoom.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
                _isFromOrigin1 = true;
                _isFromOrigin2 = true;
            }
        }

        /// <summary>
        /// 计算最大的Translation移动距离
        /// </summary>
        /// <param name="imgActualValue">_img的当前宽度/高度</param>
        /// <param name="zoomActualValue">_zoom的当前宽度/高度</param>
        /// <param name="zoomFactor">_zoom的当前缩放倍数</param>
        /// <returns>当前宽度/高度最大的Translation移动距离</returns>        
        private double ComputeMaxTranslationValue(double imgActualValue,double zoomActualValue,float zoomFactor)
        {
            return (imgActualValue * zoomFactor - zoomActualValue) / 2.0 / zoomFactor;
        }
    }
}

