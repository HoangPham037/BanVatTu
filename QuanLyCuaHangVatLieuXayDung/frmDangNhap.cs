using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft;

namespace QuanLyCuaHangVatLieuXayDung
{
    public partial class frmDangNhap : Form
    {

        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from tblNguoiDung where UserName = N'" + txtUserName.Text + "' and Password = N'" + txtPassword.Text+"'", mySqlConnection);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                frmMainMenu frmMainMenu = new frmMainMenu();
                frmMainMenu.Show();
                this.Hide();
                frmMainMenu.Exit += FrmMainMenu_Exit;
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FrmMainMenu_Exit(object sender, EventArgs e)
        {
            (sender as frmMainMenu).isExit = false;
            (sender as frmMainMenu).Close();
            this.Show();
        }
    }
}
