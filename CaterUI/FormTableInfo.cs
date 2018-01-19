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
    public partial class FormTableInfo : Form
    {
        public FormTableInfo()
        {
            InitializeComponent();
        }

        private TableInfoBll tiBll = new TableInfoBll();

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event Action Refresh;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        /// <summary>
        /// LoadSearch Hall IsFree
        /// </summary>
        private void LoadSearchList()
        {
            HallInfoBll hiBll = new HallInfoBll();

            //厅包查询
            var HallSearchList = hiBll.GetList();
            HallSearchList.Insert(0, new HallInfo()
            {
                HId = 0,
                HTitle = "全部"
            });
            ddlHallSearch.DataSource = HallSearchList;
            ddlHallSearch.ValueMember = "HId";
            ddlHallSearch.DisplayMember = "HTitle";

            //空闲查询
            List<DdlModel> ListDdl = new List<DdlModel>()
            {
                new DdlModel("-1","全部"),
                new DdlModel("0","占用"),
                new DdlModel("1","空闲")

            };
            ddlFreeSearch.DataSource = ListDdl;
            ddlFreeSearch.ValueMember = "Id";
            ddlFreeSearch.DisplayMember = "Title";

            //厅包添加
            var HallAddList = hiBll.GetList();
            ddlHallAdd.DataSource = HallAddList;
            ddlHallAdd.ValueMember = "HId";
            ddlHallAdd.DisplayMember = "HTitle";
        }


        private void Clean()
        {
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            rbFree.Checked = true;
            btnSave.Text = "添加";
            LoadSearchList();
        }

        /// <summary>
        /// LoadList 拼接了 hall、isfree条件
        /// </summary>
        private void LoadList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (ddlHallSearch.SelectedIndex > 0)
            {
                dic.Add("THallId", ddlHallSearch.SelectedValue.ToString());
            }
            if (ddlFreeSearch.SelectedIndex > 0)
            {
                dic.Add("TIsFree", ddlFreeSearch.SelectedValue.ToString());
            }

            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = tiBll.GetList(dic);
        }

        /// <summary>
        /// FormLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTableInfo_Load(object sender, EventArgs e)
        {
            LoadSearchList();
            LoadList();
        }

        /// <summary>
        /// ddlHallSear选择值变化查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlHallSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// ddlFreeSearch选择值变化查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlFreeSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// 全部查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            ddlHallSearch.SelectedIndex = 0;
            ddlFreeSearch.SelectedIndex = 0;
            LoadList();
        }

        /// <summary>
        /// 格式化“是否空闲”列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.Value = Convert.ToBoolean(e.Value) ? "空闲" : "占用";
            }
        }

        /// <summary>
        /// dgvList 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            ddlHallAdd.Text = row.Cells[2].Value.ToString();
            //虽然进行了显示格式化,但是值仍然是boolean类型
            if (Convert.ToBoolean(row.Cells[3].Value.ToString()))
            {
                rbFree.Checked = true;
            }
            else
            {
                rbUnFree.Checked = true;
            }
            btnSave.Text = "修改";
        }

        /// <summary>
        /// 保存和添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //构造接受用户输入
            TableInfo ti = new TableInfo()
            {
                TTitle = txtTitle.Text,
                THallId = Convert.ToInt32(ddlHallAdd.SelectedValue),
                TIsFree = rbFree.Checked
            };

            if (txtId.Text == "添加时无编号")
            {
                #region 添加
                if (tiBll.Add(ti))
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
                ti.TId = int.Parse(txtId.Text);
                #region 修改
                if (tiBll.Edit(ti))
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
            Clean();
            Refresh();
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clean();
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认要删除吗?", "提示:", MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK)
            {
                int id = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);
                if (tiBll.Remove(id))
                {
                    LoadList();
                    MessageBox.Show("删除成功.");
                }
                else
                {
                    MessageBox.Show("删除失败,请稍后重试......");
                }
            }
            Refresh();
        }

        /// <summary>
        /// 厅包管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddHall_Click(object sender, EventArgs e)
        {
            FormHallInfo formHallInfo = new FormHallInfo();
            
            //向委托里注册方法方法,也就是给委托赋值
            formHallInfo.MyUpdateForm += LoadList;
            formHallInfo.MyUpdateForm += LoadSearchList;
            formHallInfo.Show();
        }
    }
}
