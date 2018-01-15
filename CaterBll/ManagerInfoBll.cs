﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaterCommon;
using CaterDal;
using CaterModel;

namespace CaterBll
{
    public partial class ManagerInfoBll
    {
        //创建数据层对象
        ManagerInfoDal miDal = new ManagerInfoDal();

        public List<ManagerInfo> GetList()
        {
            
            //调用查询方法
            return miDal.GetList();
        }

        public bool Add(ManagerInfo mi)
        {
            //调用Dal层的Insert方法,完成数据插入
            return miDal.Insert(mi) > 0;
        }

        public bool Edit(ManagerInfo mi)
        {
            return miDal.Update(mi) > 0;
        }

        public bool Remove(int id)
        {
            return miDal.Delete(id) > 0;
        }

        public LoginState Login(string name,string pwd,out int type)
        {
            //设置type默认值,如果为此值时,不会使用
            type = -1;
            //根据用户名进行对象的查询
            ManagerInfo mi = miDal.GetByName(name);
            if(mi == null)
            {
                //用户名错
                return LoginState.NameError;
            }
            else
            {
                if (mi.MPwd.Equals(MD5Helper.EncrytString(pwd)))
                {
                    //密码正确
                    type = mi.MType;
                    return LoginState.OK;
                }
                else
                {
                    //密码错误
                    return LoginState.PwdError;
                }
            }
        }
    }
}
