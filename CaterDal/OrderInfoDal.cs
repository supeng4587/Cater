using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace CaterDal
{
    public partial class OrderInfoDal
    {
        public int CreateOrder(int tableId)
        {
            //插入订单数据
            //更新餐桌状态
            //写在一起执行只需要和数据库交互一次,两个sql语句用分号隔开
            string sql = "INSERT INTO OrderInfo(ODate, IsPay, TableId) VALUES(datetime('now','localtime'),0,@tId);"+
                "UPDATE TableInfo SET TIsFree = 0 WHERE TId = @tId";
            SQLiteParameter p = new SQLiteParameter("@tId", tableId);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
