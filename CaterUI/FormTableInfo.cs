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

            List<DdlModel> ListDal = new List<DdlModel>()
            {
                new DdlModel("-1","全部"),
                new DdlModel("1","空闲"),
                new DdlModel("0","占用")
            };

            ddlFreeSearch.DataSource = ListDal;
            ddlFreeSearch.ValueMember = "Id";
            ddlFreeSearch.DisplayMember = "Title";
        }

        private void LoadList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (ddlHallSearch.SelectedIndex > 0)
            {
                dic.Add("THallId",ddlHallSearch.SelectedValue.ToString());
            }
            if (ddlFreeSearch.SelectedIndex > 0)
            {
                dic.Add("TIsFree", ddlFreeSearch.SelectedIndex.ToString());
            }

            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = tiBll.GetList(dic);
        }

        private void FormTableInfo_Load(object sender, EventArgs e)
        {
            LoadSearchList();
            LoadList();
        }

        private void ddlHallSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void ddlFreeSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            ddlHallSearch.SelectedIndex = 0;
            ddlFreeSearch.SelectedIndex = 0;
            LoadList();
        }

        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
                e.Value = Convert.ToBoolean(e.Value) ? "空闲" : "占用";
            }
        }
    }
}
