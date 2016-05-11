using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TeamDailyWork.Annotations;

namespace TeamDailyWork.Models
{
    public class WorkItem:INotifyPropertyChanged
    {

        #region 类属性
        //工作项唯一标识
        public Guid Id { get; set; }

        //工作项标题
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        //工作项内容
        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                if (value == _content) return;
                _content = value;
                OnPropertyChanged();
            }
        }

        //开始时间
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (value == _startTime) return;
                _startTime = value;
                Duration = _endTime - _startTime;
                OnPropertyChanged();
            }
        }

        //结束时间
        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (value == _endTime) return;
                _endTime = value;
                Duration = _endTime - _startTime;
                OnPropertyChanged();
            }
        }

        //颜色与类型
        private WorkClassification _type;
        public WorkClassification Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        //持续时间（用来表示WorkItem在界面显示的矩形高度）
        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkItem()
        {
        }

        /// <summary>
        /// 初始化开始时间和结束时间的构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public WorkItem(Guid id, DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
            {
                throw new ArgumentException("结束事件不能在开始时间之后");
            }
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
        }

        public WorkItem(Guid id,DateTime startTime, DateTime endTime, [NotNull] string title) : this(id,startTime, endTime)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            Title = title;
        }

        public WorkItem(Guid id, DateTime startTime, DateTime endTime, [NotNull] string title, [NotNull] string content) : this(id,startTime, endTime,title)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            Content = content;
        }

        public WorkItem(Guid id,DateTime startTime, DateTime endTime, [NotNull] string title, [NotNull] string content,[NotNull]WorkClassification type) : this(id,startTime, endTime,title,content)
        {
            Type = type;
        }
        #endregion

       

        //public TimeSpan Duration => _endTime - _startTime;//C#6.0的新写法

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
