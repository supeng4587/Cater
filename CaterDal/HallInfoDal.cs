using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using CaterModel;
using CaterCommon;

namespace CaterDal
{
    public partial class HallInfoDal
    {
        /// <summary>
        /// Retrieve HallInfo list
        /// </summary>
        /// <returns></returns>
        public List<HallInfo> GetList()
        {
            List<HallInfo> list = new List<HallInfo>();
            //拼接sql,获取datatable
            string sql = "SELECT HId, HTitle, HIsDelete FROM HallInfo WHERE HIsDELETE = 1";
            DataTable dt = SqliteHelper.GetDataTable(sql);

            //遍历datatable,添加list
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new HallInfo()
                {
                    HId = int.Parse(row["HId"].ToString()),
                    HTitle = row["HTitle"].ToString()
                });
            }

            return list;
        }

        /// <summary>
        /// Insert HallInfo object
        /// </summary>
        /// <param name="hi"></param>
        /// <returns></returns>
        public int Insert(HallInfo hi)
        {
            string sql = "INSERT INTO HallInfo (HTitle, HIsDELETE) VALUES(@title, @isDelete)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",hi.HTitle),
                new SQLiteParameter("@isDelete",1)
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// Update HallInfo object
        /// </summary>
        /// <param name="hi"></param>
        /// <returns></returns>
        public int Update(HallInfo hi)
        {
            string sql = "UPDATE HallInfo SET HTitle = @title WHERE HId = @id";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",hi.HTitle),
                new SQLiteParameter("@id",hi.HId)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// Delete HallInfo object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            string sql = "UPDATE HallInfo SET HIsDelete = 0 WHERE HId =@id";
            SQLiteParameter p = new SQLiteParameter("@id", id);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
