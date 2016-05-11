using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TeamDailyWork.Models;

namespace TeamDailyWork.Services
{
    public class WorkItemService
    {
        /// <summary>
        /// 添加WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void Insert(WorkItem workItem)
        {
            string sqlStr =
                "Insert into WorkItems(Id,Title,Content,StartTime,EndTime,Type) values (@id,@title,@content,@startTime,@endTime,@type)";
            SqlParameter[] parammeters =
            {
                new SqlParameter("@id", workItem.Id),
                new SqlParameter("title", workItem.Title), new SqlParameter("@content", workItem.Content),
                new SqlParameter("startTime", workItem.StartTime), new SqlParameter("endTime", workItem.EndTime),
                new SqlParameter("@type", workItem.Type.Id)
            };
            int insertCount=SqlHelper.ExecuteNonQuery(sqlStr, CommandType.Text, parammeters);
            if (insertCount != 1)
            {
                throw new Exception("插入失败");
            }
        }


        /// <summary>
        /// 获取某一天所有的WorkItem,返回DataTable
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <returns></returns>
        public DataTable GetSelectedDateWorkItems(DateTime selectedDate)
        {
            DateTime startTime = selectedDate;
            DateTime endTime = selectedDate.AddDays(1);
            string sqlStr =
                "Select Id,Title,Content,StartTime,EndTime,Type from WorkItems where StartTime >=@startTime and EndTime <= @endTime";
            SqlParameter[] parameters =
            {
                new SqlParameter("@startTime", startTime),
                new SqlParameter("@endTime", endTime)
            };
            DataTable dt = SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text, parameters);
            return dt;
        }


        /// <summary>
        /// 更新WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void Update(WorkItem workItem)
        {
            string sqlStr =
                "Update WorkItems set Title=@title,Content=@content,StartTime=@startTime,EndTime=@endTime,Type=@type where Id=@id";
            SqlParameter[] parameters =
            {
                new SqlParameter("@title", workItem.Title),
                new SqlParameter("@content", workItem.Content),
                new SqlParameter("@startTime", workItem.StartTime),
                new SqlParameter("@endTime", workItem.EndTime),
                new SqlParameter("@type", workItem.Type.Id),
                new SqlParameter("@id", workItem.Id)
            };
            int result=SqlHelper.ExecuteNonQuery(sqlStr, CommandType.Text, parameters);
            if (result != 1)
            {
                throw new Exception("更新失败");
            }
        }


        /// <summary>
        /// 删除WorkItem
        /// </summary>
        /// <param name="workItem"></param>
        public void Delete(WorkItem workItem)
        {
            string sqlStr = "Delete from WorkItems where Id=@id";
            SqlParameter[] parameters = {new SqlParameter("@id", workItem.Id)};
            int result=SqlHelper.ExecuteNonQuery(sqlStr, CommandType.Text, parameters);
            if (result != 1)
            {
                throw new Exception("删除失败");
            }

        }
    }
}
