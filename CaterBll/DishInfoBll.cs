using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterDal;
using CaterModel;

namespace CaterBll
{
    public partial class DishInfoBll
    {
        DishInfoDal diDal = new DishInfoDal();

        public List<DishInfo> Getlist(Dictionary<string,string> dic)
        {
            return diDal.GetList(dic);
        }

        public bool Add(DishInfo di)
        {
            return diDal.Insert(di) > 0;
        }
    }
}
