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
    public partial class FormMemberTypeInfo : Form
    {
        public FormMemberTypeInfo()
        {
            InitializeComponent();
        }

        MemberTypeInfoBll mtiBll = new MemberTypeInfoBll();
        private DialogResult resule = DialogResult.Cancel;

        private void FormMemberTypeInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = mtiBll.GetList();
        }

        /// <summary>
        /// 将清理用户输入窗封装出一个方法
        /// </summary>
        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            txtDiscount.Text = "";
            btnSave.Text = "添加";
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取点击的行
            var row = dgvList.Rows[e.RowIndex];
            //将行中列的值赋给文本框
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            txtDiscount.Text = row.Cells[2].Value.ToString();
            btnSave.Text = "修改";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MemberTypeInfo mti;
            if (txtId.Text.Equals("添加时无编号"))
            {
                //添加
                //接受用户输入的值,构造对象
                try
                {
                    mti = new MemberTypeInfo()
                    {
                        MTitle = txtTitle.Text,
                        MDiscount = Convert.ToDecimal(txtDiscount.Text)
                    };
                }
                catch
                {
                    MessageBox.Show("赋值类型不正确.......");
                    Clean();
                    return;
                }
                //调用添加方法
                if (mtiBll.Add(mti))
                {
                    LoadList();
                    MessageBox.Show("添加成功.");
                }
                else
                {
                    MessageBox.Show("添加失败,请稍后重试......");
                }
            }
            else
            {
                //修改
                try
                {
                    mti = new MemberTypeInfo()
                    {
                        MId = Convert.ToInt32(txtId.Text),
                        MTitle = txtTitle.Text,
                        MDiscount = Convert.ToDecimal(txtDiscount.Text)
                    };
                }
                catch
                {
                    MessageBox.Show("赋值类型不正确.......");
                    Clean();
                    return;
                }
                //调用添加方法
                if (mtiBll.Edit(mti))
                {
                    LoadList();
                    MessageBox.Show("修改成功.");
                }
                else
                {
                    MessageBox.Show("修改失败,请稍后重试......");
                }
            }
            Clean();
            resule = DialogResult.OK;
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var row = dgvList.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells[0].Value);
            DialogResult result = MessageBox.Show("确定要删除吗?", "提示", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                if (mtiBll.Remove(id))
                {
                    LoadList();
                    MessageBox.Show("删除成功.");
                }
                else
                {
                    MessageBox.Show("删除失败,稍后重试......");
                }
            }

            resule = DialogResult.OK;
        }

        private void FormMemberTypeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = resule;
        }
    }
}
