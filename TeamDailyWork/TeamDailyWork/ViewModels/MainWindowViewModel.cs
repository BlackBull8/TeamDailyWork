using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamDailyWork.Models;
using TeamDailyWork.Services;

namespace TeamDailyWork.ViewModels
{
    public class MainWindowViewModel:NotificationObject
    {
        //new出给外部使用的对象
        private static readonly MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();

        /// <summary>
        /// 暴露给外面的方法去获取该对象
        /// </summary>
        /// <returns></returns>
        public static MainWindowViewModel GetInstance()
        {
            return _mainWindowViewModel;
        }

        /// <summary>
        /// 把默认构造函数设为空
        /// </summary>
        private MainWindowViewModel()
        {
            LoadWorkClassification();
        }


        /// <summary>
        /// 加载全部的类型
        /// </summary>
        private void LoadWorkClassification()
        {
            WorkClassifications = new Dictionary<Guid, WorkClassification>();
            WorkClassificationList = new ObservableCollection<WorkClassification>();
            WorkClassificationList = new XmlService().ReadFromXml();
            foreach (WorkClassification item in WorkClassificationList)
            {
                WorkClassifications.Add(item.Id, item);
            }
        }


        #region 关于颜色与类型设置的属性
        private Dictionary<Guid, WorkClassification> _workClassifications;

        public Dictionary<Guid, WorkClassification> WorkClassifications
        {
            get { return _workClassifications; }
            set
            {
                _workClassifications = value;
                this.OnPropertyChanged(nameof(WorkClassifications));
            }
        }


        private ObservableCollection<WorkClassification> _workClassificationList;
        public ObservableCollection<WorkClassification> WorkClassificationList
        {
            get { return _workClassificationList; }
            set
            {
                _workClassificationList = value;
                this.OnPropertyChanged(nameof(WorkClassificationList));
            }
        }

        #endregion

        #region 序列化颜色与类型的设置
        /// <summary>
        /// 序列化颜色与类型对应的设置
        /// </summary>
        public void Serialize()
        {
            new XmlService().WriteToXml(WorkClassificationList);
        }
        #endregion


    }
}
