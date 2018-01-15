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
    public partial class MemberInfoDal
    {
        public List<MemberInfo> GetList(Dictionary<string, string> dic)
        {
            //连接查询,得到会员类型的名字
            string sql = "SELECT mi.MId, mi.MName, mi.MPhone, mi.MMoney, mi.MTypeId, mi.MIsDelete, mti.MTitle AS MTypeTitle FROM MemberInfo AS mi INNER JOIN MemberTypeInfo AS mti ON mi.MTypeId = mti.MId WHERE mi.MIsDelete = 1";
            //拼接条件
            if (dic.Count > 0)
            {
                foreach (var pair in dic)
                {
                    sql += " AND " + pair.Key + " Like '%" + pair.Value + "%'";
                }
            }
            //执行得到结果集
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //定义list,完成转存
            List<MemberInfo> list = new List<MemberInfo>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MemberInfo()
                {
                    MId = Convert.ToInt32(row["MId"]),
                    MName = row["MName"].ToString(),
                    MPhone = row["MPhone"].ToString(),
                    MMoney = Convert.ToDecimal(row["MMoney"]),
                    MTypeId = Convert.ToInt32(row["MTypeId"]),
                    MTypeTitle = row["MTypeTitle"].ToString()
                });
            }
            return list;
        }

        public int Insert(MemberInfo mi)
        {
            string sql = "INSERT INTO MemberInfo(MTypeId, MName, MPhone, MMoney, MIsDelete) VALUES(@typeId, @name, @phone, @money, 1)";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter ("@typeId",mi.MTypeId),
                new SQLiteParameter("@name",mi.MName),
                new SQLiteParameter("@phone",mi.MPhone),
                new SQLiteParameter("@money",mi.MMoney)
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

    }
}
