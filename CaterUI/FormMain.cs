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
            if (type != 1)
            {
                menuManagerInfo.Visible = false;
            }
            //加载所有的厅包信息
            LoadHallInfo();
        }

        private void LoadHallInfo()
        {
            //获取所有的HallInfo厅包对象
            HallInfoBll hiBll = new HallInfoBll();
            List<HallInfo> list = hiBll.GetList();

            //遍历集合,向标签页中添加信息.遍历以前先清理一下TabPage集合
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

                //动态创建元素(列表)添加到容器(标签页)上
                ListView lvTableInfo = new ListView();

                //添加listview双击事件,完成开单功能
                lvTableInfo.DoubleClick += LvTableInfo_DoubleClick;

                //设置imagelist,让列表使用图片
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
            //获取被点的餐桌项
            ListView lv1 = sender as ListView;
            ListViewItem lvi = lv1.SelectedItems[0];

            //获取餐桌编号
            int tableId = Convert.ToInt32(lv1.SelectedItems[0].Tag);

            OrderInfoBll oiBll = new OrderInfoBll();

            if (lvi.ImageIndex == 0)
            {
                //当前餐桌空闲需要开单
                //1.开单向OrderInfo中写入，同时更新餐桌状态
                //获得订单号存到items项的Tag属性中
                lvi.Tag = oiBll.CreaterOder(tableId);

                //2.更新餐桌的图标为占用
                lv1.SelectedItems[0].ImageIndex = 1;
            }
            else
            {
                //当前餐桌已经占用，则需要点菜
                lvi.Tag = oiBll.GetOrderIdByTableTid(tableId);

            }


            //2.打开点菜页面
            FormOrderDish formOrderDish = new FormOrderDish();
            formOrderDish.Tag = lvi.Tag;
            formOrderDish.ShowDialog();//模态打开
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
            FormMemberInfo formMemberInfo = FormMemberInfo.Create();
            formMemberInfo.Show();
        }

        private void menuTableInfo_Click(object sender, EventArgs e)
        {
            FormTableInfo formTableInfo = new FormTableInfo();
            formTableInfo.Refresh += LoadHallInfo;
            formTableInfo.ShowDialog();
        }

        private void emnuDishInfo_Click(object sender, EventArgs e)
        {
            FormDishInfo formDishInfo = new FormDishInfo();
            formDishInfo.ShowDialog();
        }

    }
}
