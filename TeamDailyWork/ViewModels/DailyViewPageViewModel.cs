using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TeamDailyWork.Commands;
using TeamDailyWork.Models;
using TeamDailyWork.Services;

namespace TeamDailyWork.ViewModels
{
    public class DailyViewPageViewModel:NotificationObject
    {
        //执行Sql的对象
        private readonly WorkItemService _workItemService = new WorkItemService();

        #region 属性
        private DateTime _dateString;
        public DateTime DateString
        {
            get { return _dateString; }
            set
            {
                _dateString = value;
                this.OnPropertyChanged(nameof(DateString));
            }
        }

        private ObservableCollection<WorkItem> _workItems;
        public ObservableCollection<WorkItem> WorkItems
        {
            get { return _workItems; }
            set
            {
                _workItems = value;
                this.OnPropertyChanged(nameof(WorkItems));
            }
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }


        private ObservableCollection<TimeCellItem> _timeCells;

        public ObservableCollection<TimeCellItem> TimeCells
        {
            get { return _timeCells; }
            set
            {
                _timeCells = value;
                this.OnPropertyChanged(nameof(TimeCells));
            }
        }

        #endregion


        #region Command
        //前一天
        public DelegateCommand PreCommand { get; set; }
        public void Pre(object parameter)
        {
            DateString = DateString.AddDays(-1);
            SetTimeCells(DateString);
            LoadWorkItemInSelectedDate(DateString);
        }

        
        //后一天
        public DelegateCommand NextCommand { get; set; }
        public void Next(object parameter)
        {
            DateString = DateString.AddDays(1);
            SetTimeCells(DateString);
            LoadWorkItemInSelectedDate(DateString);
        }
        #endregion


        #region 构造函数与在构造函数中执行的方法
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DailyViewPageViewModel()
        {           
            MainWindowViewModel = MainWindowViewModel.GetInstance();         
            this.PreCommand = new DelegateCommand();
            PreCommand.ExecuteAction = new Action<object>(this.Pre);
            this.NextCommand = new DelegateCommand();
            NextCommand.ExecuteAction = new Action<object>(this.Next);
        }

        public DailyViewPageViewModel(DateTime selectedDateTime) : this()
        {
            DateString = selectedDateTime;
            ThreadPool.QueueUserWorkItem((item) => { LoadWorkItemInSelectedDate(DateString); });
            SetTimeCells(DateString);
        }

        private void SetTimeCells(DateTime dateString)
        {
            ObservableCollection<TimeCellItem> tempList = new ObservableCollection<TimeCellItem>();
            for (int i = 0; i <= 23; i++)
            {
                tempList.Add(new TimeCellItem(i, dateString));
            }
            TimeCells = tempList;
        }

        #endregion


        #region WorkItem的增删改查
        /// <summary>
        /// 添加WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void AddWorkItem(WorkItem workItem)
        {
            WorkItems.Add(workItem);
            //存储到数据库里面(Done)
            ThreadPool.QueueUserWorkItem((item) => { _workItemService.Insert(workItem); });
        }

        /// <summary>
        /// 修改WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void UpdateWorkItem(WorkItem workItem)
        {
            ThreadPool.QueueUserWorkItem((item) => { _workItemService.Update(workItem); });    
        }

        /// <summary>
        /// 加载选定日期段的WorkItems
        /// </summary>
        /// <param name="dateString"></param>
        public void LoadWorkItemInSelectedDate(DateTime dateString)
        {
            DataTable dt = _workItemService.GetSelectedDateWorkItems(dateString);
            WorkItems = DataTableToWorkItemList(dt);
        }



        //todo:其实不该放在这里的,不是这个层面的东西。解决方案：把WorkClassification和WorkClassificationList作为参数传递到WorkItemService类中的一个方法赋值。
        /// <summary>
        /// 从DataTable返回WorkItems列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private ObservableCollection<WorkItem> DataTableToWorkItemList(DataTable dt)
        {
            ObservableCollection<WorkItem> resultList = new ObservableCollection<WorkItem>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WorkItem workItem = new WorkItem();
                    if (dr["Id"] != DBNull.Value && dr["Title"] != DBNull.Value && dr["Content"] != DBNull.Value &&
                        dr["StartTime"] != DBNull.Value && dr["EndTime"] != DBNull.Value && dr["Type"] != DBNull.Value)
                    {
                        workItem.Id = (Guid)dr["Id"];
                        workItem.Title = dr["Title"].ToString();
                        workItem.Content = dr["Content"].ToString();
                        workItem.Type = MainWindowViewModel.WorkClassification[(Guid)dr["Type"]];
                        if (DateTime.Parse(dr["StartTime"].ToString()).Date == DateString.Date)
                        {
                            workItem.StartTime = DateTime.Parse(dr["StartTime"].ToString());
                        }
                        else if (DateTime.Parse(dr["StartTime"].ToString()).Date < DateString.Date)
                        {
                            workItem.StartTime = DateString;
                        }

                        if (DateTime.Parse(dr["EndTime"].ToString()).Date == DateString.Date || DateTime.Parse(dr["EndTime"].ToString()) == DateString.AddDays(1))
                        {
                            workItem.EndTime = DateTime.Parse(dr["EndTime"].ToString());
                        }
                        else if(DateTime.Parse(dr["EndTime"].ToString()).Date>DateString.Date)
                        {
                            workItem.EndTime = DateString.AddDays(1);
                        }
                    }
                    resultList.Add(workItem);
                }
            }
            return resultList;
        }


        /// <summary>
        /// 删除WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void DeleteWorkItem(WorkItem workItem)
        {
            WorkItems.Remove(workItem);
            _workItemService.Delete(workItem);
        }
        #endregion
    }
}
