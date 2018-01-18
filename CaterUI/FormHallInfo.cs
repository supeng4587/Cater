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
    public partial class FormHallInfo : Form
    {
        HallInfoBll hiBll = new HallInfoBll();

        //委托变量和int变量没什么区别,给int变量赋的是一个值,给委托变量赋的是一个方法
        public event Action MyUpdateForm;

        public FormHallInfo()
        {
            InitializeComponent();
        }

        //Load列表
        private void LoadList()
        {
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = hiBll.GetList();
        }

        //清理控件
        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            btnSave.Text = "添加";
        }

        //Load窗体
        private void FormHallInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        //datagirdview双击选择
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            btnSave.Text = "修改";
        }

        //保存修改按钮
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtId.Text == "添加时无编号")
            {
                #region Insert
                if(txtTitle.Text == "")
                {
                    MessageBox.Show("包房名称不可为空.","提示");
                    return;
                }
                HallInfo hi = new HallInfo();
                hi.HTitle = txtTitle.Text;

                if (hiBll.Add(hi))
                {
                    LoadList();
                    MessageBox.Show("添加成功.");
                }
                else
                {
                    MessageBox.Show("添加失败.请稍后重试......");
                }
                #endregion
            }
            else
            {
                #region Update
                HallInfo hi = new HallInfo();
                hi.HId = Convert.ToInt32(txtId.Text);
                hi.HTitle = txtTitle.Text;
                if (hiBll.Edit(hi))
                {
                    LoadList();
                    MessageBox.Show("修改成功.");
                }
                else
                {
                    MessageBox.Show("修改失败,请稍后再试......");
                }
                #endregion
            }
            MyUpdateForm();
            Clean();
        }

        //取消按钮
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        //逻辑删除按钮
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvList.SelectedCells[0].Value);
            DialogResult result = MessageBox.Show("确认要删除吗?", "提示", MessageBoxButtons.OKCancel);
            if(result != DialogResult.OK)
            {
                return;
            }
            if (hiBll.Remove(id))
            {
                LoadList();
                Clean();
                MessageBox.Show("删除成功.");
            }
            else
            {
                MessageBox.Show("删除失败,请稍后重试......");
            }
            MyUpdateForm();
        }
    }
}
