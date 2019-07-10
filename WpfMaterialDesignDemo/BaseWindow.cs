using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMaterialDesignDemo
{
    public class BaseWindow : Window
    {
        public static readonly DependencyProperty WindowBackGroundProperty = DependencyProperty.Register("WindowBackGround", typeof(System.Windows.Media.Brush), typeof(BaseWindow), new PropertyMetadata(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White)));
        public System.Windows.Media.Brush WindowBackGround
        {
            get { return (System.Windows.Media.Brush)GetValue(WindowBackGroundProperty); }
            set { SetValue(WindowBackGroundProperty, value); }
        }

        public static readonly DependencyProperty WindowTitleBackGroundProperty = DependencyProperty.Register("WindowTitleBackGround", typeof(System.Windows.Media.Brush), typeof(BaseWindow), new PropertyMetadata(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.BlueViolet)));
        public System.Windows.Media.Brush WindowTitleBackGround
        {
            get { return (System.Windows.Media.Brush)GetValue(WindowTitleBackGroundProperty); }
            set { SetValue(WindowTitleBackGroundProperty, value); }
        }

        public static readonly DependencyProperty WindowTitleHeightProperty = DependencyProperty.Register("WindowTitleHeight", typeof(double), typeof(BaseWindow), new PropertyMetadata(60d));
        public double WindowTitleHeight
        {
            get { return (double)GetValue(WindowTitleHeightProperty); }
            set { SetValue(WindowTitleHeightProperty, value); }
        }

        public BaseWindow()
        {
            this.MouseLeftButtonDown += BaseWindow_MouseLeftButtonDown;
        }

        /// <summary>
        /// 可拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
