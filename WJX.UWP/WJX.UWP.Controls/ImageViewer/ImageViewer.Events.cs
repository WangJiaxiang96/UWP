using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WJX.UWP.Controls
{  
    /// <summary>
    /// ImageViewer控件的事件处理
    /// </summary>
    public partial class ImageViewer
    {
        private bool _isFromOrigin1;
        private bool _isFromOrigin2;

        private void _img_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // Todo:增加图片另存为功能
        }

        private void _zoom_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_zoom.ManipulationMode.HasFlag(ManipulationModes.TranslateX))
            {
                double maxTranslationX = ComputeMaxTranslationValue(_img.ActualWidth, _zoom.ActualWidth, _zoom.ZoomFactor);

                // 判断最终Translation是否大于最大值
                if (Math.Abs(_translateTransform.X + e.Delta.Translation.X / _zoom.ZoomFactor) > maxTranslationX)
                {
                    // 直接将Translation.X定义为最大值，后面公式是为了计算向哪个方向移动
                    _translateTransform.X = maxTranslationX * (e.Delta.Translation.X >= 0 ? 1 : -1);
                }
                else
                {
                    _translateTransform.X += e.Delta.Translation.X / _zoom.ZoomFactor;
                }
            }

            if (_zoom.ManipulationMode.HasFlag(ManipulationModes.TranslateY))
            {
                double maxTranslationY = ComputeMaxTranslationValue(_img.ActualHeight, _zoom.ActualHeight, _zoom.ZoomFactor);
                // 判断最终Translation是否大于最大值
                if (Math.Abs(_translateTransform.Y + e.Delta.Translation.Y / _zoom.ZoomFactor) > maxTranslationY)
                {
                    // 直接将Translation.X定义为最大值，后面公式是为了计算向哪个方向移动
                    _translateTransform.Y = maxTranslationY * (e.Delta.Translation.Y >= 0 ? 1 : -1);
                }
                else
                {
                    _translateTransform.Y += e.Delta.Translation.Y / _zoom.ZoomFactor;
                }
            }
        }

        /// <summary>
        /// 如果图片缩放不等于100%，则恢复图片默认缩放
        /// 如果等于100%，则隐藏ImageViewer
        /// </summary>
        private void _img_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (_zoom.ZoomFactor != 1f)
            {
                ResetZoomFactor();
            }
            else
            {
                Hide();
            }
        }

        private void _img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //处理Handled防止继续触发上一层_zoom_Tapped事件
            e.Handled = true;
        }

        private void _zoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Hide();
        }

        private void _img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //AjustImageSize();
            //ResetImagePosition();
        }

        private void ImageViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        /// <summary>
        /// 根据缩放比例判断图片是否能被拖动
        /// </summary>
        private void _zoom_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_img.ActualHeight * _zoom.ZoomFactor > this.ActualHeight)
            {
                if (_isFromOrigin1)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == (ManipulationModes.System | ManipulationModes.Scale) ?
                        ManipulationModes.TranslateY : ManipulationModes.TranslateX | ManipulationModes.TranslateY;

                    _isFromOrigin1 = false;
                }
            }
            else
            {
                if (!_isFromOrigin1)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == ManipulationModes.TranslateY ?
                         ManipulationModes.System | ManipulationModes.Scale : ManipulationModes.TranslateX;

                    _translateTransform.Y = 0;
                    _translateTransform.X = 0;

                    _isFromOrigin1 = true;
                }
            }

            if (_img.ActualWidth * _zoom.ZoomFactor > this.ActualWidth)
            {
                if (_isFromOrigin2)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == (ManipulationModes.System | ManipulationModes.Scale) ?
                        ManipulationModes.TranslateX : ManipulationModes.TranslateX | ManipulationModes.TranslateY;

                    _isFromOrigin2 = false;
                }
            }
            else
            {
                if (!_isFromOrigin2)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == ManipulationModes.TranslateX ?
                         ManipulationModes.System | ManipulationModes.Scale : ManipulationModes.TranslateY;

                    _translateTransform.X = 0;
                    _translateTransform.Y = 0;

                    _isFromOrigin2 = true;
                }
            }
        }

    }
}
