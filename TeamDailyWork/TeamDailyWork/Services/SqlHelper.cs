using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamDailyWork.Services
{
    public static class SqlHelper
    {
        private readonly static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static int ExecuteNonQuery(string sql, CommandType cmdtype, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.CommandType = cmdtype;
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string sql, CommandType cmdtype, params SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.CommandType = cmdtype;
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static SqlDataReader ExecuteReader(string sql, CommandType cmdtype, params SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.CommandType = cmdtype;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdtype, params SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, connStr))
            {
                sda.SelectCommand.CommandType = cmdtype;
                if (paras != null)
                {
                    sda.SelectCommand.Parameters.AddRange(paras);
                }
                //todo:记得修改回来
                try
                {
                    sda.Fill(dt);
                }
                catch (Exception)
                {

                }
               
                return dt;
            }
        }

        //封装一个带事务的执行sql语句的方法
        public static void ExecuteNonQueryTran(List<SqlAndParameter> list)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (var SqlObject in list)
                            {
                                cmd.CommandText = SqlObject.Sql;
                                if (SqlObject.Parameters != null)
                                {
                                    cmd.Parameters.AddRange(SqlObject.Parameters);
                                }
                                cmd.CommandType = SqlObject.CmdType;
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }

    public class SqlAndParameter
    {
        public string Sql
        {
            get; set;
        }
        public SqlParameter[] Parameters
        {
            get; set;
        }
        public CommandType CmdType
        {
            get; set;
        }
    }
}
