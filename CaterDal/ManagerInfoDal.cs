using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterModel;
using System.Data;
using System.Data.SQLite;
using CaterCommon;

namespace CaterDal
{
    public partial class ManagerInfoDal
    {
        /// <summary>
        /// 查询获取结果集
        /// </summary>
        /// <returns></returns>
        public List<ManagerInfo> GetList()
        {
            //构造要查询的sql语句
            string sql = "SELECT MId,MName,MPwd,MType FROM ManagerInfo";
            //使用helper进行查询,得到结果
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //将dt中的数据转存到list中
            List<ManagerInfo> list = new List<ManagerInfo>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ManagerInfo()
                {
                    MId = Convert.ToInt32(row["MId"]),
                    MName = row["MName"].ToString(),
                    MPwd = row["MPwd"].ToString(),
                    MType = Convert.ToInt32(row["MType"])
                });
            }
            //将集合返回
            return list;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="mi">ManagerInfo类型的对象</param>
        /// <returns>受影响的行数</returns>
        public int Insert(ManagerInfo mi)
        {
            //构造Insert语句
            string sql = "INSERT INTO ManagerInfo(MName,MPwd,MType) VALUES(@name,@pwd,@type)";
            SQLiteParameter[] ps = //使用数组初始化器
            {
                new SQLiteParameter("@name",mi.MName),
                new SQLiteParameter("@pwd",MD5Helper.EncrytString(mi.MPwd)),//将密码进行md5加密
                new SQLiteParameter("@type",mi.MType)
            };
            //执行插入操作
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="mi">ManagerInfo类型的对象</param>
        /// <returns>受影响的行数</returns>
        public int Update(ManagerInfo mi)
        {
            //定义参数集合,可以动态添加元素
            List<SQLiteParameter> listPs = new List<SQLiteParameter>();
            //构造update的sql语句
            string sql = "UPDATE ManagerInfo SET MName = @name,";
            listPs.Add(new SQLiteParameter("@name", mi.MName));
            //判断是否修改密码
            if (!mi.MPwd.Equals("这是原来的密码吗?"))
            {
                sql += " MPwd = @pwd,";
                listPs.Add(new SQLiteParameter("@pwd",MD5Helper.EncrytString(mi.MPwd)));
            }
            //继续拼接sql语句
            sql += " MType = @type WHERE MId =@id";
            listPs.Add(new SQLiteParameter("@id", mi.MId));
            listPs.Add(new SQLiteParameter("@type", mi.MType));
            //执行语句并返回结果
            return SqliteHelper.ExecuteNonQuery(sql, listPs.ToArray());//ToArray将集合转化成数组
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>删除的行数</returns>
        public int Delete(int id)
        {
            string sql = "DELEtE FROM ManagerInfo WHERE MId = @id";

            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@id",id)
            };

            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// 根据用户名查找对象
        /// </summary>
        /// <param name="name">string name</param>
        /// <returns>ManagerInfo 返回null为未找到对象</returns>
        public ManagerInfo GetByName(string name)
        {
            //定义一个对象
            ManagerInfo mi = null;
            //构造sql语句和参数
            string sql = "SELECT * FROM ManagerInfo WHERE MName = @name";
            SQLiteParameter p = new SQLiteParameter("@name", name);
            //执行查询得到结果
            DataTable dt = SqliteHelper.GetDataTable(sql, p);
            //判断是否根据用户名查找到了对象
            if (dt.Rows.Count > 0)
            {
                //用户名存在
                mi = new ManagerInfo()
                {
                    MId = Convert.ToInt32(dt.Rows[0][0]),
                    MName = name,
                    MPwd = dt.Rows[0][2].ToString(),
                    MType = Convert.ToInt32(dt.Rows[0][3])
                };
            }
            else
            {
                //用户名不存在
            }
            return mi;
        }
    }
}
