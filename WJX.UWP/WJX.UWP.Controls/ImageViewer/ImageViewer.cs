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
    public sealed partial class ImageViewer : Control
    {
        #region Field

        private ScalableGrid _scale;
        private Image _img;
        private Grid _bg;

        #endregion

        public ImageViewer()
        {
            this.DefaultStyleKey = typeof(ImageViewer);
        }

        protected override void OnApplyTemplate()
        {
            if (_bg != null)
            {
            }
            if (_img != null)
            {
                _img.SizeChanged -= _img_SizeChanged;
            }
            if (_scale != null)
            {

            }

            _bg = (Grid)GetTemplateChild("BackgroundGrid");
            _img = (Image)GetTemplateChild("MainImage");
            _scale = (ScalableGrid)GetTemplateChild("MainScalableGrid");

            if (_bg != null)
            {
                _bg.ManipulationMode = ManipulationModes.All;
              //  _bg.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
            }
            if (_img != null)
            {
                _img.SizeChanged += _img_SizeChanged;
                //_img.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
                _img.ManipulationMode = ManipulationModes.All;
            }
            if (_scale != null)
            {
            }

            base.OnApplyTemplate();
        }

        private void _img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AjustImageSize();
        }

        /// <summary>
        /// 调整图片显示大小为当前控价大小的百分比（以ImageSizePercent属性的小数表示，以图片长宽中数值最大的数为基准计算）
        /// </summary>
        private void AjustImageSize()
        {
            double height = _img.ActualHeight;
            double width = _img.ActualWidth;
            if (height >= width)
            {
                _img.Height = this.ImageSizePercent * this.ActualHeight;
            }
            else
            {
                _img.Width = this.ImageSizePercent * this.ActualWidth;
            }
        }
    }
}
