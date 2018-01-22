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
    public partial class FormMemberInfo : Form
    {
        private FormMemberInfo()
        {
            InitializeComponent();
        }

        MemberInfoBll miBll = new MemberInfoBll();

        public static FormMemberInfo _formMemberInfo = null;

        public static FormMemberInfo Create()
        {
            if(_formMemberInfo == null)
            {
                _formMemberInfo = new FormMemberInfo(); 
            }
            return _formMemberInfo;
        }
        
        private void FormMemberInfo_Load(object sender, EventArgs e)
        {
            //加载会员信息
            LoadList();
            //加载会员分类信息
            LoadTypeList();
        }

        private void LoadList()
        {
            //使用键值对存储条件
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //收集用户名信息
            if(txtNameSearch.Text != "")
            {
                //需要根据名称搜索
                dic.Add("MName", txtNameSearch.Text);
            }
            //收集电话信息
            if(txtPhoneSearch.Text != "")
            {
                dic.Add("MPhone", txtPhoneSearch.Text);
            }

            //根据条件进行查询
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = miBll.GetList(dic);
            dgvList.Rows[dgvSelectedIndex].Selected = true;
        }

        private void LoadTypeList()
        {
            MemberTypeInfoBll mtiBll = new MemberTypeInfoBll();
            List<MemberTypeInfo> list = mtiBll.GetList();
            ddlType.DataSource = list;
            //设置显示值
            ddlType.DisplayMember = "MTitle";
            //设置value值
            ddlType.ValueMember = "MId";
        }

        private void txtNameSearch_Leave(object sender, EventArgs e)
        {
            //失去焦点事件
            LoadList();
        }

        private void txtPhoneSearch_TextChanged(object sender, EventArgs e)
        {
            //内容改变事件
            LoadList();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            txtNameSearch.Text = "";
            txtPhoneSearch.Text = "";
            LoadList();
        }

        private int dgvSelectedIndex = 0;

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSelectedIndex = e.RowIndex;
            var row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtNameAdd.Text = row.Cells[1].Value.ToString();
            ddlType.Text = row.Cells[2].Value.ToString();
            txtPhoneAdd.Text = row.Cells[3].Value.ToString();
            txtMoney.Text = row.Cells[4].Value.ToString();
            btnSave.Text = "修改";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //接受用户输入的数据
            MemberInfo mi = new MemberInfo()
            {
                MName = txtNameAdd.Text,
                MPhone = txtPhoneAdd.Text,
                MMoney = Convert.ToDecimal(txtMoney.Text),
                MTypeId = Convert.ToInt32(ddlType.SelectedValue)
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region 添加
                if (miBll.Add(mi))
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
                #region 修改
                mi.MId = int.Parse(txtId.Text);
                if (miBll.Edit(mi))
                {
                    LoadList();
                    MessageBox.Show("修改成功.");
                }
                else
                {
                    MessageBox.Show("修改失败,稍后再试......");
                }
                #endregion
            }
            Clean();
        }

        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtNameAdd.Text = "";
            ddlType.Text = null;
            txtPhoneAdd.Text = "";
            txtMoney.Text = "";
            btnSave.Text = "添加";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);

            DialogResult result = MessageBox.Show("确认要删除吗？", "提示", MessageBoxButtons.OKCancel);
            if(result!= DialogResult.OK)
            {
                return;
            }
            if (miBll.Remove(id))
            {
                LoadList();
                MessageBox.Show("删除成功.");
            }
            else
            {
                MessageBox.Show("删除失败,请稍后重试......");
            }
            Clean();
        }

        private void btnAddType_Click(object sender, EventArgs e)
        {
            FormMemberTypeInfo formMti = new FormMemberTypeInfo();
            //以模态窗口打开分类管理
            DialogResult result = formMti.ShowDialog();
            //根据返回值判断，是否更新分类下拉列表
            if (result == DialogResult.OK)
            {
                LoadTypeList();
                LoadList();
            }
        }

        private void FormMemberInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formMemberInfo = null;
        }
    }
}
