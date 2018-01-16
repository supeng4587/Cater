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
using CaterCommon;

namespace CaterUI
{
    public partial class FormDishInfo : Form
    {
        public FormDishInfo()
        {
            InitializeComponent();
        }

        DishInfoBll diBll = new DishInfoBll();

        private void LoadList()
        {
            //拼接条件
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (txtTitleSearch.Text != "")
            {
                dic.Add("DTitle", txtTitleSearch.Text);
            }
            if (ddlTypeSearch.SelectedValue.ToString() != "0")
            {
                dic.Add("DTypeId", ddlTypeSearch.SelectedValue.ToString());
            }
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = diBll.Getlist(dic);
        }

        private void LoadTypeList()
        {
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();
            List<DishTypeInfo> list = dtiBll.GetList();
            //向list中插入数据 使用insert制定位置插入
            #region 查询
            list.Insert(0, new DishTypeInfo() { DId = 0, DTitle = "全部" });

            ddlTypeSearch.DataSource = list;
            //显示
            ddlTypeSearch.DisplayMember = "DTitle";
            //值
            ddlTypeSearch.ValueMember = "DId";
            #endregion


            #region 添加
            ddlTypeAdd.DataSource = dtiBll.GetList(); ;
            //显示
            ddlTypeAdd.DisplayMember = "DTitle";
            //值
            ddlTypeAdd.ValueMember = "DId"; 
            #endregion

        }



        private void FormDishInfo_Load(object sender, EventArgs e)
        {
            LoadTypeList();
            LoadList();
        }

        private void txtTitleSearch_Leave(object sender, EventArgs e)
        {
            LoadList();
        }

        private void ddlTypeSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            LoadList();
        }

        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtTitleSave.Text = "";
            ddlTypeAdd.SelectedValue = "0";
            txtPrice.Text = "";
            txtChar.Text = "";
            btnSave.Text = "添加";
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvList.SelectedRows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitleSave.Text = row.Cells[1].Value.ToString();
            btnSave.Text = "修改";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DishInfo di = new DishInfo()
            {
                DTitle = txtTitleSave.Text,
                DChar = txtChar.Text,
                DPrice = Convert.ToDecimal(txtPrice.Text),
                DTypeId = Convert.ToInt32(ddlTypeAdd.SelectedValue)
            };
            if (txtId.Text == "添加时无编号")
            {
                #region 添加

                #endregion
            }
            else
            {
                #region 修改
                di.DId = Convert.ToInt32(txtId);
                #endregion
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void txtTitleSave_Leave(object sender, EventArgs e)
        {
            txtChar.Text = PinyinHelper.GetPinyin(txtTitleSave.Text);
        }
    }
}
