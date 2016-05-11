using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamDailyWork.Annotations;
using TeamDailyWork.UserControls;

namespace TeamDailyWork.Models
{
    public class TimeCellItem
    {
        //表示时间
        public static TimeCellItem[] OneDay = new TimeCellItem[24]
        {
            new TimeCellItem(00),
            new TimeCellItem(01),
            new TimeCellItem(02),
            new TimeCellItem(03),
            new TimeCellItem(04),
            new TimeCellItem(05),
            new TimeCellItem(06),
            new TimeCellItem(07),
            new TimeCellItem(08),
            new TimeCellItem(09),
            new TimeCellItem(10),
            new TimeCellItem(11),
            new TimeCellItem(12),
            new TimeCellItem(13),
            new TimeCellItem(14),
            new TimeCellItem(15),
            new TimeCellItem(16),
            new TimeCellItem(17),
            new TimeCellItem(18),
            new TimeCellItem(19),
            new TimeCellItem(20),
            new TimeCellItem(21),
            new TimeCellItem(22),
            new TimeCellItem(23)
        };

        #region 构造函数
        public TimeCellItem(int hour, DateTime selectedDate)
        {
            Time = TimeSpan.FromHours(hour);
            Date = selectedDate;
            
        }

        public TimeCellItem(int hour)
        {
            Time = TimeSpan.FromHours(hour);
        }

        public TimeCellItem()
        {

        }
        #endregion


        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }


    }
}
