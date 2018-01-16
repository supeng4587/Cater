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
    public partial class FormDishTypeInfo : Form
    {
        public FormDishTypeInfo()
        {
            InitializeComponent();
        }

        DishTypeInfoBll dtiBll = new DishTypeInfoBll();
        DishTypeInfo dti = new DishTypeInfo();

        private void FormDishTypeInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = dtiBll.GetList();
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            btnSave.Text = "修改";
        }

        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            btnSave.Text = "添加";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //根据用户输入的构造对象
            dti.DTitle = txtTitle.Text;
            //判断添加修改
            if (txtId.Text.Equals("添加时无编号"))
            {
                #region 添加
                if (dtiBll.Add(dti))
                {
                    LoadList();
                    MessageBox.Show("添加成功.");
                }
                else
                {
                    MessageBox.Show("添加失败,请稍后重试......");
                }
                #endregion
            }
            else
            {
                #region 修改 单独采集did
                dti.DId = Convert.ToInt32(txtId.Text);
                if (dtiBll.Edit(dti))
                {
                    LoadList();
                    MessageBox.Show("修改成功.");
                }
                else
                {
                    MessageBox.Show("修改失败,请稍后重试......");
                }
                #endregion
            }
            Clean();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvList.SelectedCells[0].Value);

            //删除提示
            DialogResult result = MessageBox.Show("确认要删除吗?", "提示", MessageBoxButtons.OKCancel);
            if(result != DialogResult.OK)
            {
                return;
            }

            if (dtiBll.Remove(id))
            {
                LoadList();
                Clean();
                MessageBox.Show("删除成功.");
            }
            else
            {
                MessageBox.Show("删除失败,请稍后重试......");
            }
        }
    }
}
