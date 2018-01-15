using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterDal;
using CaterModel;

namespace CaterBll
{
    public partial class MemberInfoBll
    {
        private MemberInfoDal miDal = new MemberInfoDal();

        public List<MemberInfo> GetList(Dictionary<string, string> dic)
        {
            return miDal.GetList(dic);
        }

        public bool Add(MemberInfo mi)
        {
            return miDal.Insert(mi) > 0;
        }
    }
}
