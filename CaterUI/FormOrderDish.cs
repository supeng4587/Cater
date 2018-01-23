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

        OrderInfoBll oiBll = new OrderInfoBll();
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
            dgvAllDish.AutoGenerateColumns = false;
            dgvAllDish.DataSource = diBll.Getlist(dic);
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
            int orderId = Convert.ToInt32(this.Tag);
            dgvOrderDetail.AutoGenerateColumns = false;
            dgvOrderDetail.DataSource = oiBll.GetDetailList(orderId);

            GetTotalMoneyByOrderId();
        }

        private void GetTotalMoneyByOrderId()
        {
            int orderId = Convert.ToInt32(this.Tag);
            lblMoney.Text = oiBll.GetTotalMoneyByOrderId(orderId).ToString();
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
            if (oiBll.ChooseDishes(orderId, dishId))
            {
                //点菜成功
                dgvOrderDetail.AutoGenerateColumns = false;
                dgvOrderDetail.DataSource = oiBll.GetDetailList(orderId);

            }
        }

        private void dgvOrderDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //当离开的列为count的时候执行
            if (e.ColumnIndex == 2)
            {
                //获取oid和count
                var row = dgvOrderDetail.Rows[e.RowIndex];
                int oid = Convert.ToInt32(row.Cells[0].Value);
                int count = Convert.ToInt32(row.Cells[2].Value);

                //执行更新操作
                oiBll.UpdateCountByOId(oid, count);
            }

            //重新计算总价
            GetTotalMoneyByOrderId();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            int orderId = Convert.ToInt32(this.Tag);
            decimal money = Convert.ToInt32(lblMoney.Text);

            if (oiBll.SetOrderMoney(orderId, money))
            {
                MessageBox.Show("下单成功");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int oid;
            try
            {
                oid = Convert.ToInt32(dgvOrderDetail.SelectedRows[0].Cells[0].Value);
            }
            catch
            {
                MessageBox.Show("请选中整行菜品");
                return;
            }

            DialogResult result = MessageBox.Show("是否确认删除选中菜品,并重新计算价格?", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                if (oiBll.DeleteDetailByOId(oid))
                {
                    LoadDetailInfo();

                }
                int orderId = Convert.ToInt32(this.Tag);
                decimal money = Convert.ToInt32(lblMoney.Text);

                if (oiBll.SetOrderMoney(orderId, money))
                {
                    MessageBox.Show("改单成功");
                }
            }
        }
    }
}
