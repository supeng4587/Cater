using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterModel;
using System.Data;
using System.Data.SQLite;

namespace CaterDal
{
    public partial class DishTypeInfoDal
    {
        public List<DishTypeInfo> GetList()
        {
            string sql = "SELECT DId, DTitle, DIsDelete FROM DishTypeInfo WHERE DIsDelete = 1";
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //转存集合
            List<DishTypeInfo> list = new List<DishTypeInfo>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DishTypeInfo() {
                    DId=Convert.ToInt32(row["DId"]),
                    DTitle = row["DTitle"].ToString()
                });
            }
            return list;
        }

        public int Insert(DishTypeInfo dti)
        {
            string sql = "INSERT INTO DishTypeInfo(DTitle, DIsDelete) VALUES(@title, @dIsDelete)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",dti.DTitle),
                new SQLiteParameter("@dIsDelete",1)
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int Update(DishTypeInfo dti)
        {
            string sql = "UPDATE DishTypeInfo SET DTitle = @title WHERE DId = @id";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@id",dti.DId),
                new SQLiteParameter("@title",dti.DTitle)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int Delete(int id)
        {
            string sql = "UPDATE DishTypeInfo SET DIsDelete = 0 WHERE DId = @id";
            SQLiteParameter p = new SQLiteParameter("@id", id);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
