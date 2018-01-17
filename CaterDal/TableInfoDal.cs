using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using CaterModel;

namespace CaterDal
{
    public partial class TableInfoDal
    {
        /// <summary>
        /// Retrieve TableInfo DataGridView
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public List<TableInfo> GetList(Dictionary<string, string> dic)
        {
            string sql = "SELECT ti.TId, ti.TTitle, ti.THallId, ti.TIsFree ,ti.TIsDelete, hi.HTitle FROM TableInfo AS ti INNER JOIN HallInfo AS hi ON ti.THallId = hi.HId WHERE ti.TIsDelete = 1 AND hi.HIsDelete = 1";
            if (dic.Count > 0)
            {
                foreach (var pair in dic)
                {
                    sql += " AND " + pair.Key + " = '" + pair.Value + "' ";
                }
            }

            DataTable dt = SqliteHelper.GetDataTable(sql);
            List<TableInfo> list = new List<TableInfo>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new TableInfo()
                {
                    TId = Convert.ToInt32(row["TId"]),
                    TTitle = row["TTitle"].ToString(),
                    THallId = Convert.ToInt32(row["THallId"]),
                    TIsFree = Convert.ToBoolean(row["TIsFree"]),
                    HallTitle = row["HTitle"].ToString(),
                    TIsDelete = Convert.ToBoolean(row["TIsDelete"])
                });
            }
            return list;
        }

        /// <summary>
        /// Create TableInfo object
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public int Insert(TableInfo ti)
        {
            string sql = "INSERT INTO TableInfo(TTitle, THallId, TIsFree, TIsDelete) VALUES(@title, @hallId, @isFree, 1)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",ti.TTitle),
                new SQLiteParameter("@hallId",ti.THallId),
                new SQLiteParameter("@isFree",ti.TIsFree)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// Update TableInfo object
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public int Update(TableInfo ti)
        {
            string sql = "UPDATE TableInfo SET TTitle = @title, THallId = @hallId, TIsFree = @isFree WHERE TId = @id";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter ("@title",ti.TTitle),
                new SQLiteParameter ("@hallId",ti.THallId),
                new SQLiteParameter ("@isFree",ti.TIsFree),
                new SQLiteParameter ("@id",ti.TId)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// Delete TableInfo object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delte(int id)
        {
            string sql = "UPDATE TableInfo SET TIsDelete = 0 = @isFree WHERE TId = @id";
            SQLiteParameter p = new SQLiteParameter("@id", id);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
