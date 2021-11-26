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
    public partial class frmNhanVien : Form
    {
        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private bool isNew;
        public bool isExit = true;
        public event EventHandler Exit;
        DataTable tblNhanVien;

        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            //kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            display("");
        }
        private void display(String TenNV)
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblNhanVien  WHERE TenNV Like N'%" + TenNV + "%' ORDER by MaNV";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hien thi len luoi
            tblNhanVien = new DataTable();
            tblNhanVien.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = tblNhanVien;
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtTenNV.Enabled = !edit;
            txtGioiTinh.Enabled = !edit;
            txtNgaySinh.Enabled = !edit;
            txtDiaChi.Enabled = !edit;
            txtSoDienThoai.Enabled = !edit;
            txtEmail.Enabled = !edit;
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
            txtTenNV.Clear();
            txtGioiTinh.Clear();
            txtNgaySinh.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            //chuyen con tro ve txtFirstName
            txtTenNV.Focus();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtTenNV.Focus();
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
            string MaNV = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string TenNV = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string GioiTinh = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string NgaySinh = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string DiaChi = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string SoDienThoai = dataGridView1.Rows[row].Cells[5].Value.ToString();
            string Email = dataGridView1.Rows[row].Cells[6].Value.ToString();

            string sSql = "DELETE FROM tblNhanVien WHERE (MaNV = @MaNV) and (TenNV = @TenNV) and (GioiTinh = @GioiTinh) and (NgaySinh = @NgaySinh) and (DiaChi = @DiaChi) " +
                "and (SoDienThoai = @SoDienThoai) and (Email = @Email)";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = MaNV;
            mySqlCommand.Parameters.Add("@TenNV", SqlDbType.NVarChar, 50).Value = TenNV;
            mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = GioiTinh;
            mySqlCommand.Parameters.Add("@NgaySinh", SqlDbType.VarChar, 10).Value = NgaySinh;
            mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 50).Value = DiaChi;
            mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = SoDienThoai;
            mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
            mySqlCommand.ExecuteNonQuery();
            display("");

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNV.Focus();
                return;
            }
            if (txtGioiTinh.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập giới tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGioiTinh.Focus();
                return;
            }
            if (txtNgaySinh.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập ngày sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNgaySinh.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return;
            }
            if (txtSoDienThoai.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoDienThoai.Focus();
                return;
            }
            if (txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }

            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblNhanVien (MaNV,TenNV,GioiTinh,NgaySinh,DiaChi,SoDienThoai,Email) VALUES (@MaNV,@TenNV,@GioiTinh,@NgaySinh,@DiaChi,@SoDienThoai,@Email)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);

                mySqlCommand.Parameters.Add("@MaNV", SqlDbType.NVarChar, 50).Value = txtMaNV.Text;
                mySqlCommand.Parameters.Add("@TenNV", SqlDbType.NVarChar, 50).Value = txtTenNV.Text;
                mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = txtGioiTinh.Text;
                mySqlCommand.Parameters.Add("@NgaySinh", SqlDbType.NVarChar, 10).Value = txtNgaySinh.Text;
                mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 50).Value = txtDiaChi.Text;
                mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;


                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string MaNV = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string TenNV = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string GioiTinh = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string NgaySinh = dataGridView1.Rows[row].Cells[3].Value.ToString();
                string DiaChi = dataGridView1.Rows[row].Cells[4].Value.ToString();
                string SoDienThoai = dataGridView1.Rows[row].Cells[5].Value.ToString();
                string Email = dataGridView1.Rows[row].Cells[6].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblNhanVien SET MaNV = @MaNV, TenNV = @TenNV, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, Email = @Email " +
                              "WHERE (MaNV = @MaNV1) and(TenNV = @TenNV1) and (GioiTinh = @GioiTinh1) and (NgaySinh = @NgaySinh1) and (DiaChi = @DiaChi1) and (SoDienThoai = @SoDienThoai1) and (Email = @Email1) ";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = txtMaNV.Text;
                mySqlCommand.Parameters.Add("@TenNV", SqlDbType.NVarChar, 50).Value = txtTenNV.Text;
                mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = txtGioiTinh.Text;
                mySqlCommand.Parameters.Add("@NgaySinh", SqlDbType.NVarChar, 10).Value = txtNgaySinh.Text;
                mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 50).Value = txtDiaChi.Text;
                mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text;

                mySqlCommand.Parameters.Add("@MaNV1", SqlDbType.VarChar, 15).Value = MaNV;
                mySqlCommand.Parameters.Add("@TenNV1", SqlDbType.NVarChar, 50).Value = TenNV;
                mySqlCommand.Parameters.Add("@GioiTinh1", SqlDbType.NVarChar, 5).Value = GioiTinh;
                mySqlCommand.Parameters.Add("@NgaySinh1", SqlDbType.NVarChar, 10).Value = NgaySinh;
                mySqlCommand.Parameters.Add("@DiaChi1", SqlDbType.NVarChar, 50).Value = DiaChi;
                mySqlCommand.Parameters.Add("@SoDienThoai1", SqlDbType.NVarChar, 11).Value = SoDienThoai;
                mySqlCommand.Parameters.Add("@Email1", SqlDbType.NVarChar, 50).Value = Email;
                mySqlCommand.ExecuteNonQuery();
            }
            //Truy van va hien thi lai du lieu tren luoi
            display("");
            SetControls(true);

        }
        private void FrmMainMenu_Exit(object sender, EventArgs e)
        {
            (sender as frmMainMenu).isExit = false;
            (sender as frmMainMenu).Close();
            this.Show();
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            frmMainMenu frmMainMenu = new frmMainMenu();
            frmMainMenu.Show();
            this.Hide();
            frmMainMenu.Exit += FrmMainMenu_Exit;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNV.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTenNV.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtGioiTinh.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtNgaySinh.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtDiaChi.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtSoDienThoai.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            display(txtTimKiem.Text);
        }
    }
}
