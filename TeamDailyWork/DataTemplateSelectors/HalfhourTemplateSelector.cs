using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TeamDailyWork.Models;

namespace TeamDailyWork.DataTemplateSelectors
{
    public class HalfhourTemplateSelector:DataTemplateSelector
    {
        public DataTemplate HalfhourTemplate { get; set; }
        public DataTemplate OverHalfhourTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            WorkItem workItem = (WorkItem) item;
            Type type = workItem.GetType();
            PropertyInfo startTimeProperty = type.GetProperty("StartTime");
            DateTime startTime = DateTime.Parse(startTimeProperty.GetValue(workItem, null).ToString());

            PropertyInfo endTimeProperty = type.GetProperty("EndTime");
            DateTime endTime = DateTime.Parse(endTimeProperty.GetValue(workItem, null).ToString());
            TimeSpan duration = endTime - startTime;
            if (duration <= TimeSpan.FromMinutes(30))
            {
                return HalfhourTemplate;
            }
            else
            {
                return OverHalfhourTemplate;
            }
        }
    }
}
