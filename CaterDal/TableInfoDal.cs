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
        public List<TableInfo> GetList(Dictionary<string,string> dic)
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
    }
}
