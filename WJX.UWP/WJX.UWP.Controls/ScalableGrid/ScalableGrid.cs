using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace WJX.UWP.Controls
{
    /// <summary>
    /// 支持手势缩放拖动内部元素的Grid
    /// </summary>
    public class ScalableGrid : Grid
    {
        #region Field

        public TransformGroup _transformGroup;
        public ScaleTransform _scaleTransform;
        public TranslateTransform _translateTransform;

        #endregion

        #region DependencyProperty

        public double MaxScaleRate
        {
            get { return (double)GetValue(MaxScaleRateProperty); }
            set { SetValue(MaxScaleRateProperty, value); }
        }
        public static readonly DependencyProperty MaxScaleRateProperty =
            DependencyProperty.Register("MaxScaleRate", typeof(double), typeof(ImageViewer), new PropertyMetadata(4));

        public double MinScaleRate
        {
            get { return (double)GetValue(MinScaleRateProperty); }
            set { SetValue(MinScaleRateProperty, value); }
        }
        public static readonly DependencyProperty MinScaleRateProperty =
            DependencyProperty.Register("MinScaleRate", typeof(double), typeof(ImageViewer), new PropertyMetadata(0.5));

        #endregion

        public ScalableGrid()
        {
            //设置_transformGroup
            this._scaleTransform = new ScaleTransform();
            this._translateTransform = new TranslateTransform();
            this._transformGroup = new TransformGroup();
            this._transformGroup.Children.Add(_scaleTransform);
            this._transformGroup.Children.Add(_translateTransform);

            //设置属性
            this.RenderTransform = _transformGroup;
            this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;

            //设置事件
            this.ManipulationDelta += ScalableGrid_ManipulationDelta;
            this.Loaded += ScalableGrid_Loaded;
            this.DoubleTapped += ScalableGrid_DoubleTapped;
            this.SizeChanged += ScalableGrid_SizeChanged;    
        }

        #region Event

        private void ScalableGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetCenterXY();
        }

        private void ScalableGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (_scaleTransform.ScaleX == 1 && _scaleTransform.ScaleY == 1)
            {
                this.Visibility = Visibility.Collapsed;
            }
            _scaleTransform.ScaleX = _scaleTransform.ScaleY = 1;
            this._translateTransform.X = 0;
            this._translateTransform.Y = 0;
            this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
        }

        private void ScalableGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= ScalableGrid_Loaded;
            ResetCenterXY();
        }

        private void ScalableGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_scaleTransform.ScaleX == 1 && _scaleTransform.ScaleY == 1)
            {
                this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
            }
            else
            {
                this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale ;
            }

            _scaleTransform.ScaleX *= e.Delta.Scale;
            _scaleTransform.ScaleY *= e.Delta.Scale;
            if (_scaleTransform.ScaleY < 1)
            {
                _scaleTransform.ScaleX = _scaleTransform.ScaleY = 1;
            }

            _translateTransform.X += e.Delta.Translation.X;
            _translateTransform.Y += e.Delta.Translation.Y;
            StopWhenTranslateToEdge();
        }

        #endregion

        #region Method

        /// <summary>
        /// 重设位移中心点
        /// </summary>
        private void ResetCenterXY()
        {
            this._scaleTransform.CenterX = this.ActualWidth / 2;
            this._scaleTransform.CenterY = this.ActualHeight / 2;
        }
        
        private void StopWhenTranslateToEdge()
        {
            double width = this.ActualWidth * (_scaleTransform.ScaleX - 1) / 2;

            if (_translateTransform.X > 0)
            {
                if (width < _translateTransform.X)
                {
                    _translateTransform.X = width;
                }
            }
            else if (_translateTransform.X < 0)
            {
                if (-width > _translateTransform.X)
                {
                    _translateTransform.X = -width;
                }
            }

            double height = this.ActualHeight * (_scaleTransform.ScaleY - 1) / 2;

            if (height < _translateTransform.Y)
            {
                _translateTransform.Y = height;
            }
            else if (_translateTransform.Y < 0)
            {
                if (-height > _translateTransform.Y)
                {
                    _translateTransform.Y = -height;
                }
            }
        }

        #endregion
    }
}
