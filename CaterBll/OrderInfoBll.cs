﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterDal;
using CaterModel;

namespace CaterBll
{
    public partial class OrderInfoBll
    {
        private OrderInfoDal oiDal = new OrderInfoDal();

        public int CreaterOder(int tableId)
        {
            return oiDal.CreateOrder(tableId);
        }

        public bool ChooseDishes(int orderId, int dishId)
        {
            return oiDal.ChooseDishes(orderId, dishId) > 0;

        }

        public List<OrderDetailInfo> GetDetailList(int orderId)
        {
            return oiDal.GetDetailList(orderId);
        }

        public int GetOrderIdByTableTid(int tableId)
        {
            return oiDal.GetOrderIdByTableTid(tableId);
        }

        public bool UpdateCountByOId(int oid, int count)
        {
            return oiDal.UpdateCountByOId(oid, count) > 0;
        }
    }
}
