using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WJX.UWP.Controls
{  
    /// <summary>
    /// ImageViewer控件的事件处理
    /// </summary>
    public partial class ImageViewerNew
    {
        private bool _isFromOrigin1;
        private bool _isFromOrigin2;

        private void _img_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            // Todo:增加图片另存为功能
        }

        /// <summary>
        /// 根据缩放比例判断图片是否能被拖动
        /// </summary>
        private void _zoom_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (_zoom.ZoomFactor == _zoomfactor)
            {
                return;
            }
            _zoomfactor = _zoom.ZoomFactor;

            //if(e.KeyModifiers!= Windows.System.VirtualKeyModifiers.Control)
            //{
            //    return;
            //}

            if (_img.ActualHeight * _zoom.ZoomFactor > this.ActualHeight)
            {
                if (_isFromOrigin1)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == ManipulationModes.None ?
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
                        ManipulationModes.None : ManipulationModes.TranslateX;

                    _originPosition.Y = 0;
                    _originPosition.X = 0;

                    _isFromOrigin1 = true;
                }
            }

            if (_img.ActualWidth * _zoom.ZoomFactor > this.ActualWidth)
            {
                if (_isFromOrigin2)
                {
                    _zoom.ManipulationMode =
                        _zoom.ManipulationMode == ManipulationModes.None ?
                        ManipulationModes.TranslateX : ManipulationModes.TranslateX | ManipulationModes.TranslateY;

                    _isFromOrigin2 = false;
                }
            }
            else
            {
                if (!_isFromOrigin2)
                {
                    _zoom.ManipulationMode =
                        _img.ManipulationMode == ManipulationModes.TranslateX ?
                        ManipulationModes.None : ManipulationModes.TranslateY;

                    _originPosition.X = 0;
                    _originPosition.Y = 0;

                    _isFromOrigin2 = true;
                }
            }

        }

        private void _zoom_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _img.RenderTransform = _originPosition;
            _originPosition.X += e.Delta.Translation.X / _zoom.ZoomFactor;
            _originPosition.Y += e.Delta.Translation.Y / _zoom.ZoomFactor;
        }

        /// <summary>
        /// 如果图片缩放不等于100%，则恢复图片默认缩放
        /// 如果等于100%，则隐藏ImageViewer
        /// </summary>
        private void _img_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (_zoom.ZoomFactor != 1)
            {
                _zoom.ChangeView(null, null, 1);
                RecoverImagePosition();
            }
            else
            {
                Hide();
            }
        }

        private void _img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Hide();
            //处理Handled防止继续触发上一层_zoom_Tapped事件
            e.Handled = true;
        }

        private void _zoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Hide();
        }

        private void _img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustImageSize();
            RecoverImagePosition();
        }

    }
}
