using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TeamDailyWork.Pages
{
    /// <summary>
    /// DailyViewPage.xaml 的交互逻辑
    /// </summary>
    public partial class DailyViewPage : Page
    {
        public DailyViewPage()
        {
            InitializeComponent();
            
        }

        #region 主要是用在ScrollViewer触摸时的事件
        /// <summary>
        /// 主要是用在ScrollViewer触摸时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        #region 让下拉条可受鼠标的滚珠控制

        /// <summary>
        /// 让下拉条可受鼠标的滚珠控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer viewer = Scroller;
            if (viewer == null)
            {
                return;
            }

            double num = Math.Abs(e.Delta / 2);
            double offset = e.Delta > 0 ? Math.Max(0.0,(viewer.VerticalOffset - num)) : Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            if (Math.Abs(offset - viewer.VerticalOffset) > 0.00001)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int count = Convert.ToInt32(DateTime.Now.ToString("HH"));
            //滚轴滚到目前时间段区域
            Scroller.ScrollToVerticalOffset(count * 60);
        }
    }
}
