using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeamDailyWork.Commands;
using TeamDailyWork.Models;
using TeamDailyWork.Services;

namespace TeamDailyWork.ViewModels
{
    public class WeekViewPageViewModel:NotificationObject
    {
        private DateTime _tempDateTime;

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

        private DateTime _endDateString;
        public DateTime EndDateString
        {
            get { return _endDateString; }
            set
            {
                _endDateString = value;
                this.OnPropertyChanged(nameof(EndDateString));
            }
        }


        private List<DateTime> _weekDateTime;
        public List<DateTime> WeekDateTime
        {
            get { return _weekDateTime; }
            set
            {
                if (value == null) return;
                _weekDateTime = value;
                DateString = _weekDateTime[0];
                EndDateString = _weekDateTime[6];
                this.OnPropertyChanged(nameof(WeekDateTime));
            }
        }


        private Dictionary<DateTime, DailyViewPageViewModel> _weekViewModel;
        public Dictionary<DateTime, DailyViewPageViewModel> WeekViewModel
        {
            get { return _weekViewModel; }
            set
            {
                _weekViewModel = value;
                this.OnPropertyChanged(nameof(WeekViewModel));
            }
        }

        #endregion



        #region 命令
        public DelegateCommand PreCommand { get; set; }
        public void Pre(object parameter)
        {
            _tempDateTime = _tempDateTime.AddDays(-7);
            GetWeekDate(_tempDateTime);
        }


        //后一天
        public DelegateCommand NextCommand { get; set; }
        public void Next(object parameter)
        {
            _tempDateTime = _tempDateTime.AddDays(7);
            GetWeekDate(_tempDateTime);
        }


        #endregion

        public WeekViewPageViewModel()
        {
            this.PreCommand=new DelegateCommand();
            PreCommand.ExecuteAction+= new Action<object>(Pre);

            this.NextCommand = new DelegateCommand();
            NextCommand.ExecuteAction = new Action<object>(this.Next);
        }


        public WeekViewPageViewModel(DateTime selectedDate):this()
        {
            _tempDateTime = selectedDate;
            GetWeekDate(selectedDate);
        }

        private void SetWeekViewModel()
        {
            Dictionary<DateTime, DailyViewPageViewModel> result = new Dictionary<DateTime, DailyViewPageViewModel>();
            foreach (DateTime item in WeekDateTime)
            {
                result.Add(item, new DailyViewPageViewModel(item));
            }
            WeekViewModel = result;
        }

        #region 获取一周的日期
        /// <summary>
        /// 获得一周的日期
        /// </summary>
        /// <param name="selectedDate"></param>
        private void GetWeekDate(DateTime selectedDate)
        {
            List<DateTime> dateList = new List<DateTime>();
            int wd = (int)selectedDate.DayOfWeek;//转换成int型数据

            if (wd == 0)//当等于0的时候是特例，必须单独拿出来换算
            {
                for (int i = -6; i <= 0; i++)
                {
                    DateTime currentDay = selectedDate.AddDays(i);
                    dateList.Add(currentDay);
                }
            }
            else
            {
                for (int i = 1 - wd; i < 8 - wd; i++)//循环获取
                {
                    DateTime currentDay = selectedDate.AddDays(i);
                    dateList.Add(currentDay);
                }
            }
            this.WeekDateTime = dateList;
            SetWeekViewModel();

        }
        #endregion

    }
}
