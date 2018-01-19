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
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void menuQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //判断登陆进来员工的级别,已确定是否显示menuManagerInfo图标
            int type = Convert.ToInt32(this.Tag);
            if(type != 1)
            {
                menuManagerInfo.Visible = false;
            }
            //加载所有的厅包信息
            LoadHallInfo();
        }

        private void LoadHallInfo()
        {
            //获取HallInfo,厅包对象
            HallInfoBll hiBll = new HallInfoBll();
            List<HallInfo> list = hiBll.GetList();

            //遍历集合,xiang标签页中添加信息.遍历以前先清理一下TabPage集合
            tcHallInfo.TabPages.Clear();
            TableInfoBll tiBll = new TableInfoBll();
            foreach (var hi in list)
            {
                //根据厅包对象创建标签页
                TabPage tp = new TabPage(hi.HTitle);

                //获取到当前厅包的餐桌
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("THallId", hi.HId.ToString());
                List<TableInfo> tableInfoList = tiBll.GetList(dic);
                
                //动态创建列表添加到标签页上
                ListView lvTableInfo = new ListView();

                //添加listview双击事件,完成开单功能
                lvTableInfo.DoubleClick += LvTableInfo_DoubleClick;


                lvTableInfo.LargeImageList = imageList1;
                lvTableInfo.Dock = DockStyle.Fill;

                tp.Controls.Add(lvTableInfo);
                foreach (var ti in tableInfoList)
                {
                    var lvi = new ListViewItem(ti.TTitle, ti.TIsFree ? 0 : 1);

                    //后续操作需要用到餐桌编号,所以在这里存一下
                    lvi.Tag = ti.TId;
                    lvTableInfo.Items.Add(lvi);
                }

                //将标签页加入到标签容器
                tcHallInfo.TabPages.Add(tp);
            }
        }

        private void LvTableInfo_DoubleClick(object sender, EventArgs e)
        {
            //1.开单向OrderInfo中写入
            //1.1获取餐桌编号
            var lv1 = sender as ListView;
            int tableId = Convert.ToInt32(lv1.SelectedItems[0].Tag);

            //1.2OrderInfo插入操作，同时更新餐桌状态
            OrderInfoBll oiBll = new OrderInfoBll();
            oiBll.CreaterOder(tableId);

            //1.3更新菜单项
            lv1.SelectedItems[0].ImageIndex = 1;

            //2.打开点菜页面
            FormOrderDish formOrderDish = new FormOrderDish();
            formOrderDish.Show();
        }

        private void menuManagerInfo_Click(object sender, EventArgs e)
        {
            FormManagerInfo formManagerInfo = FormManagerInfo.Create();
            formManagerInfo.Show();
            formManagerInfo.Focus();
        }

        private void menuMemberInfo_Click(object sender, EventArgs e)
        {
            //等会儿改成单例
            FormMemberInfo formMemberInfo = new FormMemberInfo();
            formMemberInfo.Show();
        }

        private void menuTableInfo_Click(object sender, EventArgs e)
        {
            FormTableInfo formTableInfo = new FormTableInfo();
            formTableInfo.Refresh += LoadHallInfo;
            formTableInfo.Show();
        }

        private void emnuDishInfo_Click(object sender, EventArgs e)
        {
            FormDishInfo formDishInfo = new FormDishInfo();
            formDishInfo.Show();
        }

    }
}
