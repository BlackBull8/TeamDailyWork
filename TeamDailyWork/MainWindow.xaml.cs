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
using TeamDailyWork.Pages;
using TeamDailyWork.ViewModels;

namespace TeamDailyWork
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly MainWindowViewModel _mainWindowViewModel = MainWindowViewModel.GetInstance();
        private bool _colorSetting;
        private bool _isCheck;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _mainWindowViewModel;

            this.ShowPageGrid.DataContext = new DailyViewPageViewModel(DateTime.Today);
            ColorsListOne.ItemsSource = new List<Color>()
            {
                Color.FromRgb(221, 222, 224),
                Color.FromRgb(175, 179, 177),
                Color.FromRgb(246, 238, 169),
                Color.FromRgb(255, 209, 219),
                Color.FromRgb(149, 196, 89),
                Color.FromRgb(120, 158, 71),
                Color.FromRgb(139, 173, 224),
                Color.FromRgb(32, 99, 186),
                Color.FromRgb(0, 156, 174),
                Color.FromRgb(0, 115, 128)
            };
        }

        #region 设置类型与颜色的对应事件
        /// <summary>
        /// 设置颜色按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            Color color = (Color)ColorsListOne.SelectedValue;
            if (CbColor.SelectedValue != null)
            {
                //修改
                WorkClassification workClassificationItem =
                    _mainWindowViewModel.WorkClassification[(Guid)CbColor.SelectedValue];
                workClassificationItem.Color = color;
            }
            else
            {
                //添加
                Guid id = Guid.NewGuid();
                WorkClassification workClassificationItem = new WorkClassification(id, CbColor.Text, color);
                _mainWindowViewModel.WorkClassification.Add(id, workClassificationItem);
                _mainWindowViewModel.WorkClassificationList.Add(workClassificationItem);
            }
            _colorSetting = true;

            TbResult.Text = "设置成功...";
            TbResult.Foreground = new SolidColorBrush(Colors.Black);
            DoubleAnimation tbResultOpacity = new DoubleAnimation();
            tbResultOpacity.From = 0;
            tbResultOpacity.To = 1;
            tbResultOpacity.AutoReverse = true;
            Duration duration = new Duration(TimeSpan.FromMilliseconds(1500));
            tbResultOpacity.Duration = duration;
            this.TbResult.BeginAnimation(TextBox.OpacityProperty, tbResultOpacity);

            CbColor.SelectedIndex = -1;
        }
        #endregion

        #region 强制焦点离开Calendar

        /// <summary>
        /// 强制焦点离开Calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
        #endregion

        #region 当关闭程序的时候,对设置进行保存
        private void RootWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_colorSetting)
            {
                _mainWindowViewModel.Serialize();
            }
        }
        #endregion

        #region 日历控件的选择事件
        //todo:要修改的日历选择事件
        ///// <summary>
        ///// 日历控件的选择事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DateTime? selectedDate = CalendarShow.SelectedDate;
        //    if (selectedDate.HasValue)
        //    {
        //        _dailyViewPageViewModel.LoadWorkItemInSelectedDate((DateTime)selectedDate);
        //        _dailyViewPageViewModel.ModifyDate((DateTime)selectedDate);
        //    }
        //}

        #endregion




        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Content.ToString() == "今日")
            {
                if (_isCheck)
                {
                    PageContainer.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                    PageContainer.Navigate(new Uri("Pages/DailyViewPage.xaml", UriKind.Relative));
                    this.ShowPageGrid.DataContext = new DailyViewPageViewModel(DateTime.Today);
                }
            }
            else if (rb.Content.ToString() == "本周")
            {
                PageContainer.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                PageContainer.Navigate(new Uri("Pages/WeekViewPage.xaml", UriKind.Relative));
                this.ShowPageGrid.DataContext = new WeekViewPageViewModel(DateTime.Today);
                _isCheck = true;
            }
        }
    }
}
