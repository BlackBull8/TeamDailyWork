﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeamDailyWork.Models;
using TeamDailyWork.Services;

namespace TeamDailyWork.ViewModels
{
    public class MainWindowViewModel:NotificationObject
    {
        //new出给外部使用的对象
        private static readonly MainWindowViewModel SingleMainWindowViewModel = new MainWindowViewModel();

        #region 关于颜色与类型设置的属性
        private Dictionary<Guid, WorkClassification> _workClassification;

        public Dictionary<Guid, WorkClassification> WorkClassification
        {
            get { return _workClassification; }
            set
            {
                _workClassification = value;
                OnPropertyChanged(nameof(WorkClassification));
            }
        }


        private ObservableCollection<WorkClassification> _workClassificationList;
        public ObservableCollection<WorkClassification> WorkClassificationList
        {
            get { return _workClassificationList; }
            set
            {
                _workClassificationList = value;
                OnPropertyChanged(nameof(WorkClassificationList));
            }
        }

        #endregion



        /// <summary>
        /// 暴露给外面的方法去获取该对象
        /// </summary>
        /// <returns></returns>
        public static MainWindowViewModel GetInstance()
        {
            return SingleMainWindowViewModel;
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
            WorkClassification = new Dictionary<Guid, WorkClassification>();
            WorkClassificationList = new ObservableCollection<WorkClassification>();
            WorkClassificationList = new XmlService().ReadFromXml();
            foreach (WorkClassification item in WorkClassificationList)
            {
                WorkClassification.Add(item.Id, item);
            }
        }


       

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
