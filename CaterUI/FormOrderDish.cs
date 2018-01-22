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
        public FormOrderDish()
        {
            InitializeComponent();
        }

        private void FormOrderDish_Load(object sender, EventArgs e)
        {
            LoadDishTypeInfo();
            LoadDetailInfo();
            LoadDishInfo();
        }

        private void LoadDishInfo()
        {
            //拼接查询条件
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (txtTitle.Text != "")
            {
                dic.Add("DChar", txtTitle.Text);
            }
            if (ddlType.SelectedValue.ToString() != "0")
            {
                dic.Add("DTypeId", ddlType.SelectedValue.ToString());
            }

            //查询菜品显示到dgvAllDish中
            DishInfoBll diBll = new DishInfoBll();
            dgvAllDish.DataSource = diBll.Getlist(dic);
            dgvAllDish.AutoGenerateColumns = false;

        }

        private void LoadDishTypeInfo()
        {
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();
            List<DishTypeInfo> list = dtiBll.GetList();

            list.Insert(0, new DishTypeInfo() { DId = 0, DTitle = "全部" });

            ddlType.ValueMember = "DId";
            ddlType.DisplayMember = "DTitle";
            ddlType.DataSource = list;
        }

        private void LoadDetailInfo()
        {
            OrderInfoBll oiBll = new OrderInfoBll();
            int orderId = Convert.ToInt32(this.Tag);
            dgvOrderDetail.AutoGenerateColumns = false;
            dgvOrderDetail.DataSource = oiBll.GetDetailList(orderId);
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void dgvAllDish_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获得订单编号
            int orderId = Convert.ToInt32(this.Tag);

            //获得菜品编号
            int dishId = Convert.ToInt32(dgvAllDish.Rows[e.RowIndex].Cells[0].Value);

            //执行点菜操作
            OrderInfoBll oiBll = new OrderInfoBll();
            if (oiBll.ChooseDishes(orderId, dishId))
            {
                //点菜成功
                dgvOrderDetail.AutoGenerateColumns = false;
                dgvOrderDetail.DataSource = oiBll.GetDetailList(orderId);

            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void btnOrder_Click(object sender, EventArgs e)
        {

        }
    }
}
