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

        TableInfoBll tiBll = new TableInfoBll();

        /// <summary>
        /// LoadSearch Hall IsFree
        /// </summary>
        private void LoadSearchList()
        {
            HallInfoBll hiBll = new HallInfoBll();
            var Halllist = hiBll.GetList();

            Halllist.Insert(0, new HallInfo()
            {
                HId = 0,
                HTitle = "全部"
            });
            ddlHallSearch.DataSource = Halllist;
            ddlHallSearch.ValueMember = "HId";
            ddlHallSearch.DisplayMember = "HTitle";

            List<DdlModel> ListDdl = new List<DdlModel>()
            {
                new DdlModel("-1","全部"),
                new DdlModel("0","占用"),
                new DdlModel("1","空闲")

            };

            ddlFreeSearch.DataSource = ListDdl;
            ddlFreeSearch.ValueMember = "Id";
            ddlFreeSearch.DisplayMember = "Title";
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
        /// dgvList 双击时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            ddlHallAdd.SelectedText = row.Cells[2].Value.ToString();
            
        }

        /// <summary>
        /// 保存和添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 厅包管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddHall_Click(object sender, EventArgs e)
        {

        }
    }
}
