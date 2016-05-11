using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamDailyWork.Controls;
using TeamDailyWork.Models;
using TeamDailyWork.ViewModels;

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

            double num = Math.Abs((int)(e.Delta / 2));
            double offset = 0.0;
            if (e.Delta > 0)
            {
                offset = Math.Max((double)0.0, (double)(viewer.VerticalOffset - num));
            }
            else
            {
                offset = Math.Min(viewer.ScrollableHeight, viewer.VerticalOffset + num);
            }
            if (offset != viewer.VerticalOffset)
            {
                viewer.ScrollToVerticalOffset(offset);
                e.Handled = true;
            }
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int count = Convert.ToInt32(System.DateTime.Now.ToString("HH"));
            //滚轴滚到目前时间段区域
            Scroller.ScrollToVerticalOffset(count * 60);
        }
    }
}
