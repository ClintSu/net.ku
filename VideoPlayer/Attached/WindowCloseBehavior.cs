using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;


namespace VideoPlayer.Attached
{
    public class WindowCloseBehavior : Behavior<Window>
    {
        #region 圆角值

        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.RegisterAttached("Close", typeof(bool?), typeof(WindowCloseBehavior), new PropertyMetadata(OnCloseChanged));


        public bool Close
        {
            get { return (bool)GetValue(CloseProperty); }
            set { SetValue(CloseProperty, value); }
        }

        private static void OnCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = ((WindowCloseBehavior)d).AssociatedObject;
            var newValue = ((bool)e.NewValue);
            if (newValue)
            {
                window.Close();
            }
        }
        #endregion
    }
}
