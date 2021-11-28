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

namespace QuanLyCuaHangVatLieuXayDung
{
    public partial class frmMainMenu : Form
    {
        public bool isExit = true;
        public event EventHandler Exit;

        public frmMainMenu()
        {
            InitializeComponent();
        }
        private void loạiHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoaiHang loaiHang = new frmLoaiHang();
            loaiHang.ShowDialog();
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhaCC nhaCC = new frmNhaCC();
            nhaCC.ShowDialog();
        }

        private void frmMainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(isExit)
            Application.Exit();
        }

        private void frmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isExit)
            {
                if (MessageBox.Show("Bạn có muốn thoát chương trình", "Cảnh báo...", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit(this, new EventArgs());
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
           
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhanVien nhanvien = new frmNhanVien();
            nhanvien.ShowDialog();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKhachHang frmKhachHang = new frmKhachHang(); 
            frmKhachHang.ShowDialog();
        }

        private void hàngHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHangHoa frmHang = new frmHangHoa();  
            frmHang.ShowDialog();
        }
    }
}
