using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterDal;

namespace CaterBll
{
    public partial class OrderInfoBll
    {
        private OrderInfoDal oiDal = new OrderInfoDal(); 

        public bool CreaterOder(int tableId)
        {
            return oiDal.CreateOrder(tableId) > 0;
        }
    }
}
