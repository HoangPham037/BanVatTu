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
    public partial class frmHoaDonXuat : Form
    {
        String sqlCon = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        SqlConnection SqlConnection = null;
        SqlCommand Command;
        DataTable dt;
        private bool isNew;
        public frmHoaDonXuat()
        {
            InitializeComponent();
        }

        private void frmHoaDonXuat_Load(object sender, EventArgs e)
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
            LoadComboBoxKH();
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
        void LoadComboBoxKH()
        {
            Command = new SqlCommand("select * from tblKhachHang", SqlConnection);
            SqlDataReader sqlDataReader = Command.ExecuteReader();
            dt = new DataTable();
            dt.Load(sqlDataReader);
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "SoDienThoai";
            cboKhachHang.DataSource = dt;
        }
        private void display()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblHoaDonXuat";
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
            cboKhachHang.Enabled = !edit;
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

            string sSql = "DELETE FROM tblHoaDonXuat WHERE (SoHDXuat = @SoHDXuat) and (MaKH = @MaKH) and (MaNV = @MaNV) " +
                    "and (NgayLap = @NgayLap)";
            Command = new SqlCommand(sSql, SqlConnection);
            Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = SoHD;
            Command.Parameters.Add("@MaKH", SqlDbType.VarChar, 15).Value = MaNCC;
            Command.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = MaNV;
            Command.Parameters.Add("@NgayLap", SqlDbType.VarChar, 10).Value = NgayLap;
            Command.ExecuteNonQuery();
            display();
            MessageBox.Show("Xóa thành công");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSHD.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSHD.Focus();
                return;
            }
            if (txtNgayLap.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập giới tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNgayLap.Focus();
                return;
            }
            if (cboKhachHang.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboKhachHang.Focus();
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
                string sSql = "INSERT INTO tblHoaDonXuat (SoHDXuat,MaKH,MaNV,NgayLap) VALUES (@SoHDXuat,@MaKH,@MaNV,@NgayLap)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = txtSHD.Text;
                Command.Parameters.Add("@MaKH", SqlDbType.VarChar, 15).Value = cboKhachHang.Text;
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
                string SoDHXuat = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string MaKH = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string MaNV = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string NgayLap = dataGridView1.Rows[row].Cells[3].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblHoaDonNhap SET SoHDXuat = @SoHDXuat, MaNCC = @MaKH, MaNV = @MaNV, NgayLap = @NgayLap" +
                              "WHERE (SoHDXuat = @SoHDXuat1) and (MaKH = @MaKH1) and (MaNV = @MaNV1) and (NgayLap = @NgayLap1)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = txtSHD.Text;
                Command.Parameters.Add("@MaKH", SqlDbType.VarChar, 15).Value = cboKhachHang.Text;
                Command.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = cboNV.Text;
                Command.Parameters.Add("@NgayLap", SqlDbType.VarChar, 10).Value = txtNgayLap.Text;

                Command.Parameters.Add("@SoHDXuat1", SqlDbType.VarChar, 15).Value = SoDHXuat;
                Command.Parameters.Add("@MaKH1", SqlDbType.VarChar, 15).Value = MaKH;
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
            txtSHD.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            cboKhachHang.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cboNV.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtNgayLap.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            frmCTPX cTPX = new frmCTPX();
            cTPX.SHD = txtSHD.Text;
            cTPX.NgayLap = txtNgayLap.Text;
            cTPX.TenKH = cboKhachHang.Text;
            cTPX.TenNV  =cboNV.Text;
            cTPX.SDT = txtSDT.Text;
            cTPX.Show();

        }

        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSDT.Text = cboKhachHang.SelectedValue.ToString();
        }
    }
}
