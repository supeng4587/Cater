using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CaterBll;
using CaterModel;

namespace CaterUI
{
    public partial class FormOrderDish : Form
    {
        private static FormOrderDish _formOrderDish = null;

        private FormOrderDish()
        {
            InitializeComponent();
        }

        public static FormOrderDish Create()
        {
            if(_formOrderDish == null)
            {
                FormOrderDish _formOrderDish = new FormOrderDish();
            }
            return _formOrderDish;
        }

        private void FormOrderDish_Load(object sender, EventArgs e)
        {
            LoadDishTypeInfo();
            LoadDishInfo();
        }

        private void LoadDishInfo()
        {
            //拼接查询条件
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if(txtTitle.Text != "")
            {
                dic.Add("DChar", txtTitle.Text);
            }
            if(ddlType.SelectedIndex != 0)
            {
                dic.Add("DTypeId", ddlType.SelectedValue.ToString());
            }

            //查询菜品显示到dgvAllDish中
            DishInfoBll diBll = new DishInfoBll();
            dgvAllDish.AutoGenerateColumns = false;
            dgvAllDish.DataSource = diBll.Getlist(dic);
        }

        private void LoadDishTypeInfo()
        {
            DishTypeInfoBll dti = new DishTypeInfoBll();
            List<DishTypeInfo> list = dti.GetList();

            list.Insert(0, new DishTypeInfo() { DId = 0, DTitle = "全部" });

            ddlType.DataSource = list;
            ddlType.ValueMember = "DId";
            ddlType.DisplayMember = "DTitle";
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }
    }
}
