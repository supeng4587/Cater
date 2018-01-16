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
    public partial class DishInfoDal
    {
        public List<DishInfo> GetList(Dictionary<string,string> dic)
        {
            string sql = "SELECT "+
                "di.DId, di.DTypeId, di.DTitle, dti.DTitle AS DTypeTitle, di.DChar, di.DPrice, di.DIsDelete " +
                "FROM DishInfo AS di INNER JOIN DishTypeInfo AS dti ON di.DTypeId = dti.DId " +
                "WHERE di.DIsDelete = 1 AND dti.DIsDelete =1";
            if (dic.Count != 0)
            {
                foreach (var pair in dic)
                {
                    sql += " AND di." + pair.Key + " LIKE '%" + pair.Value + "%'"; 
                }
                
            }

            DataTable dt = SqliteHelper.GetDataTable(sql);
            
            List<DishInfo> list = new List<DishInfo>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DishInfo()
                {
                    DId = Convert.ToInt32(row["DId"]),
                    DTypeId = Convert.ToInt32(row["DTypeId"]),
                    DTitle = row["DTitle"].ToString(),
                    DTypeTitle = row["DTypeTitle"].ToString(),
                    DChar = row["DChar"].ToString(),
                    DPrice =Convert.ToDecimal(row["DPrice"])
                });
            }

            return list;
        }

        public int Insert(DishInfo di)
        {
            string sql = "INSERT INTO (DTypeId, DTitle, DChar, DPrice ,DIsDelete) VALUES(@typeId, @title, @char, @price, @isDelete)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@typeId",di.DTypeId),
                new SQLiteParameter("@title",di.DTitle),
                new SQLiteParameter("@char",di.DChar),
                new SQLiteParameter("@price",di.DPrice),
                new SQLiteParameter("@isDelete",1)
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }


    }
}
