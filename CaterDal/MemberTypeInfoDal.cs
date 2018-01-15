using CaterModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterDal
{
    public partial class MemberTypeInfoDal
    {
        /// <summary>
        /// 查询MIsDelete状态等于真的数据
        /// </summary>
        /// <returns></returns>
        public List<MemberTypeInfo> GetList()
        {
            //查询未删除的数据,MIsDelete是Boolean,1是真0是假
            string sql = "SELECT MId, MTitle, MDiscount FROM MemberTypeInfo WHERE MIsDelete = 1";
            //执行查询返回DataTable表格
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //定义几个对象
            List<MemberTypeInfo> list = new List<MemberTypeInfo>();

            //遍历表格,将数据转存到集合中
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MemberTypeInfo() {
                    MId = Convert.ToInt32(row["MId"]),
                    MTitle = row["MTitle"].ToString(),
                    MDiscount = Convert.ToDecimal(row["MDiscount"])
                });
            }
            return list;
        }

        /// <summary>
        /// insert MemberTypeInfo对象
        /// </summary>
        /// <param name="mti"></param>
        /// <returns></returns>
        public int Insert(MemberTypeInfo mti)
        {
            //构造insert语句
            string sql = "INSERT INTO MemberTypeInfo(MTitle, MDiscount, MIsDelete) VALUES(@title, @discount, 1)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",mti.MTitle),
                new SQLiteParameter("@discount",mti.MDiscount),
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// update MemberTypeInfo对象
        /// </summary>
        /// <param name="mti"></param>
        /// <returns></returns>
        public int Update(MemberTypeInfo mti)
        {
            //构造update语句
            string sql = "UPDATE MemberTypeInfo SET MTitle = @tilte, MDiscount = @disconunt WHERE MId = @id";
            //构造参数
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@tilte",mti.MTitle),
                new SQLiteParameter("@disconunt",mti.MDiscount),
                new SQLiteParameter("@id",mti.MId)
            };
            //执行
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int Delete(int id)
        {
            string sql = "UPDATE MemberTypeInfo SET MIsDelete = 0 WHERE MId = @id";
            SQLiteParameter p = new SQLiteParameter("@id", id);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
