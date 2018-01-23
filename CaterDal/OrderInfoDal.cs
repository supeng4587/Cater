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
    public partial class OrderInfoDal
    {
        public int CreateOrder(int tableId)
        {
            //插入订单数据
            //更新餐桌状态
            //写在一起执行,只需要和数据库交互一次.
            //两个sql语句用分号隔开
            //下订单
            string sql = "INSERT INTO OrderInfo(ODate, IsPay, TableId) VALUES(datetime('now','localtime'),0,@tableId);"+
                //更新餐桌状态
                "UPDATE TableInfo SET TIsFree = 0 WHERE TId = @tableId;" +
                //获取最新的订单编号
                "SELECT OId FROM OrderInfo WHERE TableId = @tableId AND IsPay = 0 ORDER BY OId DESC LIMIT 0,1";
            SQLiteParameter p = new SQLiteParameter("@tableId", tableId);

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(sql, p));
        }

        public int ChooseDishes(int orderId,int dishId)
        {
            string sql = "SELECT COUNT(*) FROM OrderDetailInfo WHERE DishId = @dishId AND OrderId = @orderId";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@orderId",orderId),
                new SQLiteParameter("@dishId",dishId)
            };
            int index = Convert.ToInt32(SqliteHelper.ExecuteScalar(sql, ps));

            if (index > 0)
            {
                //已点菜品增加份数
                sql = "UPDATE OrderDetailInfo SET Count = Count + 1 WHERE DishId = @dishId AND OrderId = @orderId";
            }
            else
            {
                //点新蔡
                sql = "INSERT INTO OrderDetailInfo(OrderId, DishId, Count) VALUES(@orderId, @dishId, 1)";
            }
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int GetOrderIdByTableTid(int tableId)
        {
            string sql = "SELECT OId FROM OrderInfo WHERE IsPay = 0 AND TableId = @tableId";
            SQLiteParameter p = new SQLiteParameter("@tableId", tableId);

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(sql,p));
        }

        public List<OrderDetailInfo> GetDetailList(int orderId)
        {
            string sql = "SELECT odi.OId, di.DTitle, di.DPrice, odi.Count FROM DishInfo AS di INNER JOIN OrderDetailInfo AS odi ON di.DId = odi.DishId WHERE odi.OrderId = @orderid";
            SQLiteParameter p = new SQLiteParameter("@orderid", orderId);

             DataTable dt =SqliteHelper.GetDataTable(sql, p);

            List<OrderDetailInfo> list = new List<OrderDetailInfo>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new OrderDetailInfo()
                {
                    OId =Convert.ToInt32(row["OId"]),
                    DTitle = row["DTitle"].ToString(),
                    DPrice = Convert.ToDecimal(row["DPrice"]),
                    Count = Convert.ToInt32(row["Count"])
                });
            }
            return list;
        }

        public int UpdateCountByOId(int oid,int count)
        {
            string sql = "UPDATE OrderDetailInfo SET Count = @count WHERE OId = @oid";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@count",count),
                new SQLiteParameter("@oid",oid)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public decimal GetTotalMoneyByOrderId(int orderId)
        {
            string sql = "SELECT SUM(odi.Count*di.DPrice) FROM OrderDetailInfo AS odi INNER JOIN DishInfo AS di ON odi.DishId = di.DId WHERE odi.OrderId = @orderId";
            SQLiteParameter p = new SQLiteParameter("@orderId", orderId);

            try
            {
                return Convert.ToDecimal(SqliteHelper.ExecuteScalar(sql, p));
            }
            catch
            {
                return 0;
            }
        }

        public int SetOrderMoney(int orderId, decimal money)
        {
            string sql = "UPDATE OrderInfo set OMoney = @money WHERE OId =@oid";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@oid",orderId),
                new SQLiteParameter("@money",money)
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int DeleteDetailByOId(int oid)
        {
            string sql = "DELETE FROM OrderDetailInfo WHERE OId = @oid";
            SQLiteParameter p = new SQLiteParameter("oid", oid);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }

        public int pay(bool isUseMoney, int memberId, decimal payMoney,int orderId,decimal discount)
        {
            //创建数据库的连接对象
            using (SQLiteConnection conn = new SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CaterConn"].ConnectionString))
            {
                int result = 0;
                //由数据库链接对象创建事物
                conn.Open();
                SQLiteTransaction tran = conn.BeginTransaction();

                //创建command对象
                SQLiteCommand cmd = new SQLiteCommand();
                //将命令对象启用事物
                cmd.Transaction = tran;
                //执行各种命令
                string sql = "";
                SQLiteParameter[] ps;
                //1.根据是否使用余额决定扣款方式
                try
                {
                    if (isUseMoney)
                    {
                        //使用余额
                        sql = "UPDATE MemberInfo SET MMoney = MMoney - @payMoney WHERE MId=@mid";
                        ps = new SQLiteParameter[]
                        {
                        new SQLiteParameter("@payMoney",payMoney),
                        new SQLiteParameter("@mid",memberId)
                        };
                        cmd.CommandText = sql;
                        cmd.Parameters.AddRange(ps);
                        result +=cmd.ExecuteNonQuery();
                    }

                    //2.将订单状态改为IsPay=1
                    sql = "UPDATE OrderInfo SET IsPay = 1, MemberId = @mid, Discount = @discount WHERE OId =@oid";
                    ps = new SQLiteParameter[]
                    {
                    new SQLiteParameter("@mid",memberId),
                    new SQLiteParameter("@discount",discount),
                    new SQLiteParameter("@oid",orderId)
                    };
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(ps);
                    result += cmd.ExecuteNonQuery();

                    //3.将餐桌状态改为IsFree=1
                    sql = "UPDATE TableInfo SET TIsFree = 1 WHERE TId = (SELECT TableId FROM OrderInfo WHERE oid = @oid)";

                    SQLiteParameter p = new SQLiteParameter("@oid",orderId);
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(p);
                    result += cmd.ExecuteNonQuery();

                    //提交事物
                    tran.Commit();
                }
                catch
                {
                    result = 0;
                    //回滚事物
                    tran.Rollback();
                }
                return result;
            }
        }
    }
}
