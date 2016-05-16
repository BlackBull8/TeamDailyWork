using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TeamDailyWork.Controls
{
    public class WorkCell:Control
    {
        static WorkCell()
        {
            //到Theme中的Generic中寻找默认样式
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkCell), new FrameworkPropertyMetadata(typeof(WorkCell)));
            //对WorkCell控件启用操作事件
            IsManipulationEnabledProperty.OverrideMetadata(typeof(WorkCell),new FrameworkPropertyMetadata(true));
            //设置WrokCell控件能用做拖放操作的目标
            AllowDropProperty.OverrideMetadata(typeof(WorkCell), new FrameworkPropertyMetadata(true));
        }


        //设置一个Key以便读取
        private static readonly string DataName = typeof (WorkCell).FullName + ".Time";

        #region 声明注册路由事件

        //OnDrop抛出去的路由事件
        public static readonly RoutedEvent GotTimeEvent = EventManager.RegisterRoutedEvent("GotTime",
            RoutingStrategy.Bubble, typeof(WorkCellGetTimeHandler), typeof(WorkCell));

        public event WorkCellGetTimeHandler GotTime
        {
            add { AddHandler(GotTimeEvent, value); }
            remove { RemoveHandler(GotTimeEvent, value); }
        }

        //DragOver所抛出去的路由事件事件
        public static readonly RoutedEvent ReceivingTimeEvent = EventManager.RegisterRoutedEvent("ReceivingTime",
            RoutingStrategy.Bubble, typeof(WorkCellGetTimeHandler), typeof(WorkCell));

        public event WorkCellGetTimeHandler ReceivingTime
        {
            add { AddHandler(ReceivingTimeEvent, value); }
            remove { AddHandler(ReceivingTimeEvent, value); }
        }

        #endregion

        #region 声明注册依赖项属性

        //日期的依赖项属性
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register("Day", typeof (DateTime),
            typeof (WorkCell), new PropertyMetadata(default(DateTime)));

        public DateTime Day
        {
            get { return (DateTime) GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        //时间的依赖项属性
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof (TimeSpan),
            typeof (WorkCell), new PropertyMetadata(default(TimeSpan)));

        public TimeSpan Time
        {
            get { return (TimeSpan) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset",
            typeof (TimeSpan), typeof (WorkCell), new PropertyMetadata(default(TimeSpan)));

        public TimeSpan Offset {
            get { return (TimeSpan) GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion

        #region 前台绑定的静态字段 时间段
        public static TimeSpan ZeroOffset = TimeSpan.FromHours(0);
        public static TimeSpan HalfHourOffset = TimeSpan.FromMinutes(30);
        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkCell()
        {
            AllowDrop = true;//可在此绑定，也可在前台绑定
            MouseDown += WorkCell_MouseDown; ;
            Drop += WorkCell_Drop;
            DragOver += WorkCell_DragOver;
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataObject data = new DataObject();
            data.SetData(DataName, Day + Time + Offset);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Link);
        }

        /// <summary>
        /// 时刻发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkCell_DragOver(object sender, DragEventArgs e)
        {
            DateTime time1 =(DateTime)e.Data.GetData(DataName);
            DateTime time2 = Day + Time + Offset;
            RaiseEvent(new WorkCellGotTimeEventArgs(ReceivingTimeEvent, time1, time2));
        }

       
        /// <summary>
        /// 拖动结束所执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkCell_Drop(object sender, DragEventArgs e)
        {
            DateTime time1 = (DateTime)e.Data.GetData(DataName);
            DateTime time2 = Day + Time + Offset;
            RaiseEvent(new WorkCellGotTimeEventArgs(GotTimeEvent, time1, time2));
        }
        #endregion
    }

    public delegate void WorkCellGetTimeHandler(object sender,WorkCellGotTimeEventArgs e);

    public class WorkCellGotTimeEventArgs : RoutedEventArgs
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public WorkCellGotTimeEventArgs(RoutedEvent routedEvent)
        {
            RoutedEvent = routedEvent;
        }

        public WorkCellGotTimeEventArgs(RoutedEvent routedEvent, DateTime time1, DateTime time2) : this(routedEvent)
        {
            if (time1 < time2)
            {
                StartTime = time1;
                EndTime = time2.AddMinutes(30);
            }
            else
            {
                StartTime = time2;
                EndTime = time1.AddMinutes(30);
            }
        }

        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            WorkCellGetTimeHandler handler = (WorkCellGetTimeHandler) genericHandler;
            handler(genericTarget, this);
        }

    }
}
