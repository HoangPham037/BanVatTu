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
    public partial class frmHDNhap : Form
    {
        String sqlCon = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        SqlConnection SqlConnection = null;
        SqlCommand Command;
        DataTable dt;
        private bool isNew;
        public frmHDNhap()
        {
            InitializeComponent();
        }

        private void demo_Load(object sender, EventArgs e)
        {
            try
            {
                if (SqlConnection == null)
                {
                    SqlConnection = new SqlConnection(sqlCon);
                }
                if (SqlConnection.State == ConnectionState.Closed)
                {
                    SqlConnection.Open();
                    MessageBox.Show("Kết nối thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            display();
            LoadComboBoxNV();
            LoadComboBoxNCC();

        } 
        void LoadComboBoxNV()
        {
            Command = new SqlCommand("select TenNV from tblNhanVien", SqlConnection);
            SqlDataReader sqlDataReader = Command.ExecuteReader();
            dt = new DataTable();
            dt.Load(sqlDataReader);
            cboNV.DisplayMember = "TenNV";
            cboNV.DataSource = dt;
        }
        void LoadComboBoxNCC()
        {
            Command = new SqlCommand("select TenNCC from tblNhaCC", SqlConnection);
            SqlDataReader sqlDataReader = Command.ExecuteReader();
            dt = new DataTable();
            dt.Load(sqlDataReader);
            cboNCC.DisplayMember = "TenNCC";
            cboNCC.DataSource = dt;
        }
        private void display()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblHoaDonNhap";
             Command = new SqlCommand(sSql, SqlConnection);
            SqlDataReader mySqlDataReader = Command.ExecuteReader();

            //Hien thi len luoi
            DataTable dt = new DataTable();
            dt.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = dt;
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtSHD.Enabled = !edit;
            txtNgayLap.Enabled = !edit;
            cboNCC.Enabled = !edit;
            cboNV.Enabled = !edit;
            //thiet lap trang thai Enable/Disable cho cac nut an
            btnAdd.Enabled = edit;
            btnEdit.Enabled = edit;
            btnDelete.Enabled = edit;
            btnSave.Enabled = !edit;
            btnCancel.Enabled = !edit;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //thiet lap cac trang thai
            isNew = true;
            SetControls(false);
            //xoa trang cac textboxes
            txtSHD.Clear();
            txtNgayLap.Clear();
            //chuyen con tro ve txtFirstName
            txtSHD.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtSHD.Focus();
            isNew = false;
            SetControls(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Xác nhận có xóa không
            DialogResult dialog;
            dialog = MessageBox.Show("Bạn có chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.No) return;
            //Lấy dữ liệu trên lưới
            int row = dataGridView1.CurrentRow.Index;
            string SoHD = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string MaNCC = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string MaNV = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string NgayLap = dataGridView1.Rows[row].Cells[3].Value.ToString();

            string sSql = "DELETE FROM tblHoaDonNhap WHERE (SoHDNhap = @SoHDNhap) and (MaNCC = @MaNCC) and (MaNV = @MaNV) " +
                    "and (NgayLap = @NgayLap)";
            Command = new SqlCommand(sSql, SqlConnection);
            Command.Parameters.Add("@SoHDNhap", SqlDbType.VarChar, 15).Value = SoHD;
            Command.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = MaNCC;
            Command.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = MaNV;
            Command.Parameters.Add("@NgayLap", SqlDbType.VarChar, 10).Value = NgayLap;
            Command.ExecuteNonQuery();
            display();
            MessageBox.Show("Xóa thành công");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSHD.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSHD.Focus();
                return;
            }
            if (txtNgayLap.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập giới tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNgayLap.Focus();
                return;
            }
            if (cboNCC.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị chọn nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNCC.Focus();
                return;
            }
            if (cboNV.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboNV.Focus();
                return;
            }

            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblHoaDonNhap (SoHDNhap,MaNCC,MaNV,NgayLap) VALUES (@SoHDNhap,@MaNCC,@MaNV,@NgayLap)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@SoHDNhap", SqlDbType.VarChar, 15).Value = txtSHD.Text;
                Command.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = cboNCC.Text;
                Command.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = cboNV.Text;
                Command.Parameters.Add("@NgayLap", SqlDbType.VarChar, 10).Value = txtNgayLap.Text;

                Command.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string SoDHNhap = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string MaNCC = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string MaNV = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string NgayLap = dataGridView1.Rows[row].Cells[3].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblHoaDonNhap SET SoHDNhap = @SoHDNhap, MaNCC = @MaNCC, MaNV = @MaNV, NgayLap = @NgayLap" +
                              "WHERE (SoHDNhap = @SoHDNhap1) and (MaNCC = @MaNCC1) and (MaNV = @MaNV1) and (NgayLap = @NgayLap1)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@SoHDNhap", SqlDbType.VarChar, 15).Value = txtSHD.Text;
                Command.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = cboNCC.Text;
                Command.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = cboNV.Text;
                Command.Parameters.Add("@NgayLap", SqlDbType.VarChar, 10).Value = txtNgayLap.Text;

                Command.Parameters.Add("@SoHDNhap1", SqlDbType.VarChar, 15).Value = SoDHNhap;
                Command.Parameters.Add("@MaNCC1", SqlDbType.VarChar, 15).Value = MaNCC;
                Command.Parameters.Add("@MaNV1", SqlDbType.VarChar, 15).Value = MaNV;
                Command.Parameters.Add("@NgayLap1", SqlDbType.VarChar, 10).Value = NgayLap;
                Command.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Truy van va hien thi lai du lieu tren luoi
            display();
            SetControls(true);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtSHD.Text     = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            cboNCC.Text     = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cboNV.Text      = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtNgayLap.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }


        private void btnNhap_Click(object sender, EventArgs e)
        {
            frmChiTietHDNhap chiTietHDNhap = new frmChiTietHDNhap();
            chiTietHDNhap.SHD = txtSHD.Text;
            chiTietHDNhap.NgayLap = txtNgayLap.Text;
            chiTietHDNhap.TenNCC = cboNCC.Text;
            chiTietHDNhap.TenNV = cboNV.Text;
            chiTietHDNhap.Show();
        }
    }
}
