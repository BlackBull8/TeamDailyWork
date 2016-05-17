using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TeamDailyWork.Controls;
using TeamDailyWork.Models;
using TeamDailyWork.ViewModels;

namespace TeamDailyWork.UserControls
{
    /// <summary>
    /// DailyViewUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DailyViewUserControl : UserControl
    {
        public DailyViewUserControl()
        {
            InitializeComponent();
        }

        public static Dictionary<DateTime, DailyViewUserControl> DateUserControlDict =
            new Dictionary<DateTime, DailyViewUserControl>();

        private DailyViewPageViewModel _dailyViewPageViewModel;


        private void DailyViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            _dailyViewPageViewModel = this.DataContext as DailyViewPageViewModel;
            if (_dailyViewPageViewModel != null)
            {
                if (DateUserControlDict.Keys.Contains(_dailyViewPageViewModel.DateString))
                {
                    DateUserControlDict[_dailyViewPageViewModel.DateString] = this;
                }
                else
                {
                    DateUserControlDict.Add(_dailyViewPageViewModel.DateString, this);
                }
            }
        }

        #region 显示BlockCover
        /// <summary>
        /// 选定时间段，并在有颜色的矩形显示在上面表示选定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkCell_ReceivingTime(object sender, Controls.WorkCellGotTimeEventArgs e)
        {
            DateTime startDate = e.StartTime.Date;
            DateTime endDate = e.EndTime.Date;
            foreach (DateTime tempDate in DateUserControlDict.Keys)
            {
                if (tempDate >= startDate && tempDate <= endDate)
                {
                    //开始时间和结束时间在相同的Date中
                    if (startDate == endDate)
                    {
                        BlockCover.Visibility = Visibility.Visible;
                        BlockCover.Height = (e.EndTime - e.StartTime).TotalMinutes;
                        TranslateTransform translateTransform = new TranslateTransform(0,
                            (e.StartTime.TimeOfDay.TotalMinutes));
                        BlockCover.RenderTransform = translateTransform;
                    }
                    //开始时间和结束时间在不同的Date中
                    else
                    {
                        //对开始时间的Rectangle进行赋值
                        if (tempDate == e.StartTime.Date)
                        {
                            Rectangle rectangleStart = DateUserControlDict[tempDate].BlockCover;
                            rectangleStart.Visibility = Visibility.Visible;
                            rectangleStart.Height = (e.StartTime.Date.AddDays(1) - e.StartTime).TotalMinutes;
                            TranslateTransform translateTransformStart = new TranslateTransform(0,
                                e.StartTime.TimeOfDay.TotalMinutes);
                            rectangleStart.RenderTransform = translateTransformStart;
                            continue;

                        }
                        //对结束时间的Rectangle进行赋值
                        else if (tempDate == e.EndTime.Date)
                        {
                            Rectangle rectangleEnd = DateUserControlDict[tempDate].BlockCover;
                            rectangleEnd.Visibility = Visibility.Visible;
                            rectangleEnd.Height = (e.EndTime - e.EndTime.Date).TotalMinutes;
                            TranslateTransform translateTransformEnd = new TranslateTransform(0, 0);
                            rectangleEnd.RenderTransform = translateTransformEnd;
                            continue;
                        }
                        else
                        {
                            Rectangle rectangleMiddle = DateUserControlDict[tempDate].BlockCover;
                            rectangleMiddle.Height = 24 * 60;
                            rectangleMiddle.RenderTransform = new TranslateTransform(0, 0);
                        }
                    }

                }
                else
                {
                    //清空没有在时间段内的
                    Rectangle rectangleHidden = DateUserControlDict[tempDate].BlockCover;
                    rectangleHidden.Visibility = Visibility.Hidden;
                }
            }
        }
        #endregion

        #region 获取开始时间与结束时间并弹出Popup窗体
        //开始时间
        private DateTime _startTime;
        //结束时间
        private DateTime _endTime;

        /// <summary> 
        /// 获取开始时间和结束时间，并弹出Popup窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkCell_GotTime(object sender, WorkCellGotTimeEventArgs e)
        {
            _startTime = e.StartTime;
            _endTime = e.EndTime;
            //todo:待解决跨天问题
            if (_startTime.Date != _endTime.Date)
            {
                TbDateShowMulti.Text = _startTime.ToString("MM月dd日 (dddd) HH:mm") + "-" + _endTime.ToString("MM月dd日 (dddd) HH:mm");
                AddWorkPopUpMulti.IsOpen = true;
                return;
            }
            TbTimeShow.Text = _startTime.ToString("HH:mm") + " - " + _endTime.ToString("HH:mm");
            AddWorkPopUpSingle.IsOpen = true;
            //Console.WriteLine("开始时间：" + e.StartTime + ",结束时间：" + e.EndTime);
        }

        #endregion

        #region 创建WorkItem按钮事件
        /// <summary>
        /// 创建WorkItem按钮事件，并关闭Popup窗体，清空Popup窗体内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateAsBtn_Click(object sender, RoutedEventArgs e)
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), _startTime, _endTime, _startTime.ToString("HH:mm") + "-" + _endTime.ToString("HH:mm"), TbContentShow.Text, ((WorkClassification)TypeToColorList.SelectedItem)); _dailyViewPageViewModel?.AddWorkItem(workItem,true);
            AddWorkPopUpSingle.IsOpen = false;
            BlockCover.Visibility = Visibility.Hidden;
            TbContentShow.Text = "";
            TypeToColorList.SelectedIndex = -1;
        }
        #endregion

        #region 设置编辑窗体的内容
        private WorkItem _workItem;

        /// <summary>
        /// 编辑WorkItem的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkItemBtn_Click(object sender, RoutedEventArgs e)
        {
            _workItem = (WorkItem)((Button)sender).Tag;

            //EditWorkPopUp的时间段
            SetCbTime(_workItem.StartTime.ToString("HH:mm"), _workItem.EndTime.ToString("HH:mm"));
            TbContentShowEdit.Text = _workItem.Content;
            TypeToColorListEdit.SelectedItem = _workItem.Type;
            EditWorkPopUp.IsOpen = true;
        }

        
        /// <summary>
        /// 设置编辑Popup的开始时间和结束时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        private void SetCbTime(String startTime, String endTime)
        {
            List<string> times = new List<string>();
            List<string> hours = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    hours.Add("0" + i.ToString());
                }
                else
                {
                    hours.Add(i.ToString());
                }
            }

            List<string> minutes = new List<string>() { "00", "30" };

            foreach (string hour in hours)
            {
                foreach (string minute in minutes)
                {
                    times.Add(hour + ":" + minute);
                }
            }

            CbStartTime.ItemsSource = times;
            CbEndTime.ItemsSource = times;
            CbStartTime.SelectedItem = startTime;
            CbEndTime.SelectedItem = endTime;
        }
       
        /// <summary>
        /// EditPopup窗体中修改按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditAsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Parse(CbStartTime.SelectedValue.ToString()) >
               DateTime.Parse(CbEndTime.SelectedValue.ToString()))
            {
                DoubleAnimation tbResultOpacity = new DoubleAnimation();
                tbResultOpacity.From = 1;
                tbResultOpacity.To = 0;
                Duration duration = new Duration(TimeSpan.FromMilliseconds(5000));
                tbResultOpacity.Duration = duration;
                TbErrorMessage.BeginAnimation(TextBox.OpacityProperty, tbResultOpacity);

            }
            else
            {
                string date = _workItem.StartTime.ToString("yyyy-MM-dd") + " ";
                _workItem.Title = CbStartTime.SelectedValue + "-" + CbEndTime.SelectedValue;
                _workItem.Content = TbContentShowEdit.Text;
                _workItem.StartTime = DateTime.Parse(date + CbStartTime.SelectedValue);
                _workItem.EndTime = DateTime.Parse(date + CbEndTime.SelectedValue);
                _workItem.Type = (WorkClassification)TypeToColorListEdit.SelectedItem;
                EditWorkPopUp.IsOpen = false;
                _dailyViewPageViewModel.UpdateWorkItem(_workItem);
            }
        }
        #endregion

        #region Popup窗体的关闭按钮事件
        private void AddClose_Click(object sender, RoutedEventArgs e)
        {
            AddWorkPopUpSingle.IsOpen = false;
            AddWorkPopUpMulti.IsOpen = false;
        }

        private void EditClose_Click(object sender, RoutedEventArgs e)
        {
            EditWorkPopUp.IsOpen = false;
        }
        #endregion

        #region 删除WorkItem事件
        /// <summary>
        /// 删除WorkItem事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗?", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                WorkItem deleteWorkItem = (WorkItem)(((Button)sender).Tag);
                //todo:利用id查出WorkItem的具体日期，并在字典中查找
                WorkItem workItem = _dailyViewPageViewModel.QueryDeletingWorkItem(deleteWorkItem.Id);
                if (workItem != null)
                {
                    for (DateTime dt = workItem.StartTime.Date; dt <= workItem.EndTime.Date;)
                    {
                        (DateUserControlDict[dt].DataContext as DailyViewPageViewModel)?.DeleteWorkItem(deleteWorkItem);
                        for (int i = 0; i < (DateUserControlDict[dt].DataContext as DailyViewPageViewModel) ?.WorkItems.Count; i++)
                        {
                            if ((DateUserControlDict[dt].DataContext as DailyViewPageViewModel)?.WorkItems[i].Id ==
                                workItem.Id)
                            {
                                (DateUserControlDict[dt].DataContext as DailyViewPageViewModel)?.WorkItems.RemoveAt(i);
                            }
                        }
                        dt = dt.AddDays(1);
                    }
                }
                _dailyViewPageViewModel.DeleteWorkItem(workItem);
            }
        }
        #endregion

        #region Popup窗体关闭事件（把覆盖矩形的Visibility设为Hidden）
        private void AddWorkPopUpSingle_Closed(object sender, EventArgs e)
        {
            this.BlockCover.Visibility = Visibility.Hidden;
        }

        private void AddWorkPopUpMulti_Closed(object sender, EventArgs e)
        {
            foreach (DailyViewUserControl item in DateUserControlDict.Values)
            {
                item.BlockCover.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        private void GenerateAsBtnMulti_Click(object sender, RoutedEventArgs e)
        {
            WorkItem workItem = new WorkItem(Guid.NewGuid(), _startTime, _endTime, _startTime.ToString("HH:mm") + "-" + _endTime.ToString("HH:mm"), TbContentShowMulti.Text, ((WorkClassification)TypeToColorListMulti.SelectedItem));

            for (DateTime dt = workItem.StartTime.Date; dt <= workItem.EndTime.Date;)
            {
                WorkItem item = new WorkItem();
                item.Id = workItem.Id;
                item.Content = workItem.Content;
                item.Title = workItem.Title;
                item.Type = workItem.Type;
                if (dt == workItem.StartTime.Date)
                {
                    item.StartTime = workItem.StartTime;
                    item.EndTime = workItem.StartTime.Date.AddDays(1);
                }
                else if (dt == workItem.EndTime.Date)
                {
                    item.StartTime = workItem.EndTime.Date;
                    item.EndTime = workItem.EndTime;
                }
                else
                {
                    item.StartTime = dt;
                    item.EndTime = dt.AddDays(1);
                }
                (DateUserControlDict[dt].DataContext as DailyViewPageViewModel)?.WorkItems.Add(item);
                dt = dt.AddDays(1);
            }
            _dailyViewPageViewModel?.AddWorkItem(workItem,false);
            AddWorkPopUpMulti.IsOpen = false;
            BlockCover.Visibility = Visibility.Hidden;
            TbContentShowMulti.Text = "";
            TypeToColorListMulti.SelectedIndex = -1;
        }
    }
}
