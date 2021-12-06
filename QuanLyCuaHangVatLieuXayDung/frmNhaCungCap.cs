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
    public partial class frmNhaCungCap : Form
    {
        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private bool isNew;
        public event EventHandler Exit;
        DataTable tblNhaCC;
        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            //kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            display("");
        }
        private void display(String TenNCC)
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblNhaCC  WHERE TenNCC Like N'%" + TenNCC + "%' ORDER by MaNCC";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hien thi len luoi
            tblNhaCC = new DataTable();
            tblNhaCC.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = tblNhaCC;
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtMaNCC.Enabled = !edit;
            txtTenNCC.Enabled = !edit;
            txtGioiTinh.Enabled = !edit;
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
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtGioiTinh.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            //chuyen con tro ve txtFirstName
            txtTenNCC.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtTenNCC.Focus();
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
            string MaNCC = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string TenNCC = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string DiaChi = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string SoDienThoai = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string Email = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string GioiTinh = dataGridView1.Rows[row].Cells[5].Value.ToString();

            string sSql = "DELETE FROM tblNhaCC WHERE (MaNCC = @MaNCC) and (TenNCC = @TenNCC) and (DiaChi = @DiaChi) " +
                    "and (SoDienThoai = @SoDienThoai) and (Email = @Email) and (GioiTinh = @GioiTinh)";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = MaNCC;
            mySqlCommand.Parameters.Add("@TenNCC", SqlDbType.NVarChar, 50).Value = TenNCC;
            mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 250).Value = DiaChi;
            mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = SoDienThoai;
            mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 35).Value = Email;
            mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = GioiTinh;
            mySqlCommand.ExecuteNonQuery();
            display("");
            MessageBox.Show("Xóa thành công");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTenNCC.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNCC.Focus();
                return;
            }
            if (txtGioiTinh.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập giới tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGioiTinh.Focus();
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
                string sSql = "INSERT INTO tblNhaCC (MaNCC,TenNCC,GioiTinh,DiaChi,SoDienThoai,Email) VALUES (@MaNCC,@TenNCC,@GioiTinh,@DiaChi,@SoDienThoai,@Email)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = txtMaNCC.Text;
                mySqlCommand.Parameters.Add("@TenNCC", SqlDbType.NVarChar, 50).Value = txtTenNCC.Text;
                mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 250).Value = txtDiaChi.Text;
                mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 35).Value = txtEmail.Text;
                mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = txtGioiTinh.Text;
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string MaNCC = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string TenNCC = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string DiaChi = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string SoDienThoai = dataGridView1.Rows[row].Cells[3].Value.ToString();
                string Email = dataGridView1.Rows[row].Cells[4].Value.ToString();
                string GioiTinh = dataGridView1.Rows[row].Cells[5].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblNhaCC SET MaNCC = @MaNCC, TenNCC = @TenNCC, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, Email = @Email, GioiTinh = @GioiTinh " +
                              "WHERE (MaNCC = @MaNCC1) and (TenNCC = @TenNCC1) and (DiaChi = @DiaChi1) and (SoDienThoai = @SoDienThoai1) and (Email = @Email1) and (GioiTinh = @GioiTinh1) ";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaNCC", SqlDbType.VarChar, 15).Value = txtMaNCC.Text;
                mySqlCommand.Parameters.Add("@TenNCC", SqlDbType.NVarChar, 50).Value = txtTenNCC.Text;
                mySqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 250).Value = txtDiaChi.Text;
                mySqlCommand.Parameters.Add("@SoDienThoai", SqlDbType.NVarChar, 11).Value = txtSoDienThoai.Text;
                mySqlCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 35).Value = txtEmail.Text;
                mySqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar, 5).Value = txtGioiTinh.Text;

                mySqlCommand.Parameters.Add("@MaNCC1", SqlDbType.VarChar, 15).Value = MaNCC;
                mySqlCommand.Parameters.Add("@TenNCC1", SqlDbType.NVarChar, 50).Value = TenNCC;
                mySqlCommand.Parameters.Add("@DiaChi1", SqlDbType.NVarChar, 250).Value = DiaChi;
                mySqlCommand.Parameters.Add("@SoDienThoai1", SqlDbType.NVarChar, 11).Value = SoDienThoai;
                mySqlCommand.Parameters.Add("@Email1", SqlDbType.NVarChar, 35).Value = Email;
                mySqlCommand.Parameters.Add("@GioiTinh1", SqlDbType.NVarChar, 5).Value = GioiTinh;
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!", "Thông báo...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Truy van va hien thi lai du lieu tren luoi
            display("");
            SetControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtMaNCC.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTenNCC.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDiaChi.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtSoDienThoai.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtGioiTinh.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            display(txtTimKiem.Text);
        }
    }
}
