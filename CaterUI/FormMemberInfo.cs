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
        public FormMemberInfo()
        {
            InitializeComponent();
        }

        MemberInfoBll miBll = new MemberInfoBll();

        
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

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

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
                //修改 
                #endregion
            }
        }
    }
}
