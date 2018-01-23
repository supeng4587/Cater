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
    public partial class FormOrderPay : Form
    {
        public FormOrderPay()
        {
            InitializeComponent();
        }
        private OrderInfoBll oiBll = new OrderInfoBll();
        private int orderId;
        public event Action Refresh;

        private void FormOrderPay_Load(object sender, EventArgs e)
        {
            //通过标签获取传递过来的订单编号
            orderId = Convert.ToInt32(this.Tag);

            gbMember.Enabled = false;

            GetMoneyByOrderId();
        }

        private void GetMoneyByOrderId()
        {
            lblPayMoney.Text = lblPayMoneyDiscount.Text = oiBll.GetTotalMoneyByOrderId(orderId).ToString();
        }

        private void cbkMember_CheckedChanged(object sender, EventArgs e)
        {
            gbMember.Enabled = cbkMember.Checked;
        }

        private void LoadMember()
        {
            //根据会员编号和会员电话进行查询
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if(txtId.Text != "")
            {
                dic.Add("mi.MId",txtId.Text);
            }
            if (txtPhone.Text != "")
            {
                dic.Add("mi.MPhone",txtPhone.Text);
            }

            MemberInfoBll miBll = new MemberInfoBll();
            var list = miBll.GetList(dic);
            if (list.Count > 0)
            {
                //根据信息查询到了会员
                MemberInfo mi = list[0];
                lblMoney.Text = mi.MMoney.ToString();
                lblTypeTitle.Text = mi.MTypeTitle.ToString();
                lblDiscount.Text = mi.MDiscount.ToString();

                //计算折扣价
                lblPayMoneyDiscount.Text = (Convert.ToDecimal(lblPayMoney.Text) * Convert.ToDecimal(lblDiscount.Text)).ToString();
            }
            else
            {
                MessageBox.Show("会员信息有误"); 
            }
        }

        private void txtId_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void btnOrderPay_Click(object sender, EventArgs e)
        {
            //1.根据是否使用余额决定扣款方式
            //2.将订单状态改为IsPay=1
            //3.将餐桌状态改为IsFree=1

            if(oiBll.pay(cbkMoney.Checked, int.Parse(txtId.Text), Convert.ToDecimal(lblPayMoneyDiscount.Text), orderId, Convert.ToDecimal(lblDiscount.Text)))
            {
                MessageBox.Show("付款成功,感谢惠顾.");
                Refresh();
                this.Close();
            }
            else
            {
                MessageBox.Show("结账失败,TMD");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
