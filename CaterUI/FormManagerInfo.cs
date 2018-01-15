﻿using System;
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
    public partial class FormManagerInfo : Form
    {
        private FormManagerInfo()
        {
            InitializeComponent();
        }
        #region 典型的窗体单例模式

        //实现窗体的单例
        private static FormManagerInfo _form;
        public static FormManagerInfo Create()
        {
            if (_form == null)
            {
                _form = new FormManagerInfo();
            }
            return _form;
        }

        private void FormManagerInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //与单例保持一致,清理静态字段_form
            _form = null;
        } 
        #endregion

        //创建业务逻辑层对象
        ManagerInfoBll miBll = new ManagerInfoBll();

        private void FormManagerInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            //禁用dgvList自动生成列
            dgvList.AutoGenerateColumns = false;
            //调用方法获取数据,绑定到列表的数据源上
            dgvList.DataSource = miBll.GetList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //接受用户输入
            ManagerInfo mi = new ManagerInfo()
            {
                MName = txtName.Text,
                MPwd = txtPwd.Text,
                MType = rb1.Checked ? 1 : 0//经理值为1,店员值为0
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region 添加
                if (miBll.Add(mi))
                {
                    MessageBox.Show("添加成功.");
                    LoadList();
                }
                else
                {
                    MessageBox.Show("添加失败,请稍后再试......");
                }
                #endregion
            }
            else
            {
                #region 修改
                mi.MId = int.Parse(txtId.Text);
                if (miBll.Edit(mi))
                {
                    MessageBox.Show("添加成功.");
                    LoadList();
                }
                else
                {
                    MessageBox.Show("修改失败,请稍后再试......");
                }
            }
            #endregion
            CleanText();
        }

        private void CleanText()
        {
            //清除文本框中的值
            txtName.Text = "";
            txtPwd.Text = "";
            rb2.Checked = true;
            btnSave.Text = "添加";
            txtId.Text = "添加时无编号";
        }

        //对类型列进行格式化,将数值转化成文字显示
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //对类型进行格式化处理
            if (e.ColumnIndex == 2)
            {
                //根据类型判断内容
                e.Value = Convert.ToInt32(e.Value) == 1 ? "经理" : "店员";
            }
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //根据当前点击的单元格,找到行和列,进行赋值
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtName.Text = row.Cells[1].Value.ToString();
            if (row.Cells[2].Value.ToString().Equals("1"))
            {
                rb1.Checked = true; //值为1,则经理选中
            }
            else
            {
                rb2.Checked = true; //值为0,则店员选中
            }
            txtPwd.Text = "这是原来的密码吗?";

            btnSave.Text = "修改";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CleanText();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //获取选中的行
            var rows = dgvList.SelectedRows;
            if (rows.Count > 0)
            {
                //删除前的确认提示
                DialogResult result = MessageBox.Show("确定要删除吗?", "提示", MessageBoxButtons.OKCancel);
                if(result == DialogResult.Cancel)
                {
                    return;
                }
                //获取选中行id
                int id = int.Parse(rows[0].Cells[0].Value.ToString());
                //调用删除的操作
                if (miBll.Remove(id))
                {
                    MessageBox.Show("删除成功");
                    LoadList();
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的行");
            }
        }
    }
}
