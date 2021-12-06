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

    public partial class frmTTTK : Form
    {
        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private bool isNew;
        public bool isExit = true;
        DataTable tblNguoiDung;
        public frmTTTK()
        {
            InitializeComponent();
        }
        private void display(String MaNV)
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblNguoiDung  WHERE MaNV Like N'%" + MaNV + "%' ORDER by MaNV";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hien thi len luoi
            tblNguoiDung = new DataTable();
            tblNguoiDung.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = tblNguoiDung;

        }
        public void LOADCOMBOBOXNV()
        {
            mySqlCommand = new SqlCommand("select MaNV from tblNhanVien", mySqlConnection);
            SqlDataReader sqlDataReader = mySqlCommand.ExecuteReader();
            tblNguoiDung = new DataTable();
            tblNguoiDung.Load(sqlDataReader);
            cboNV.DisplayMember = "MaNV";
            cboNV.DataSource = tblNguoiDung;
        }

        private void frmTTTK_Load(object sender, EventArgs e)
        {

            //kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            display("");
            LOADCOMBOBOXNV();
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtUser.Enabled = !edit;
            txtPass.Enabled = !edit;
            txtQuyen.Enabled = !edit;
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
            txtUser.Clear();
            txtPass.Clear();
            txtQuyen.Clear();
            //chuyen con tro ve txtFirstName
            txtUser.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtUser.Focus();
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
            string User = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string Pass = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string MaNV = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string Quyen = dataGridView1.Rows[row].Cells[3].Value.ToString();

            string sSql = "DELETE FROM tblNguoiDung WHERE (UserName = @UserName) and (Password = @Password) and (MaNV = @MaNV) and (Quyen = @Quyen)";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 15).Value = User;
            mySqlCommand.Parameters.Add("@Password", SqlDbType.VarChar, 15).Value = Pass;
            mySqlCommand.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = MaNV;
            mySqlCommand.Parameters.Add("@Quyen", SqlDbType.VarChar, 10).Value = Quyen;
            mySqlCommand.ExecuteNonQuery();
            display("");
            MessageBox.Show("Xóa thành công");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập username!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUser.Focus();
                return;
            }
            if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPass.Focus();
                return;
            }
            if (txtQuyen.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập quyền", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuyen.Focus();
                return;
            }

            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblNguoiDung (UserName,Password,MaNV,Quyen) VALUES (@UserName,@Password,@MaNV,@Quyen)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);

                mySqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 15).Value = txtUser.Text;
                mySqlCommand.Parameters.Add("@Password", SqlDbType.VarChar, 15).Value = txtPass.Text;
                mySqlCommand.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = cboNV.Text;
                mySqlCommand.Parameters.Add("@Quyen", SqlDbType.VarChar, 10).Value = txtQuyen.Text;
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string User = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string Pass = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string MaNV = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string Quyen = dataGridView1.Rows[row].Cells[3].Value.ToString();

                //Update
                //dung tham so
                string sSql = "UPDATE tblNguoiDung SET UserName = @UserName, Password = @Password, MaNV = @MaNV, DonViTinh = @DonViTinh" +
                    "WHERE (UserName=@UserName1) and (Password = @Password1) and (MaNV = @MaNV1) and (Quyen = @Quyen1)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar, 15).Value = txtUser.Text;
                mySqlCommand.Parameters.Add("@Password", SqlDbType.VarChar, 15).Value = txtPass.Text;
                mySqlCommand.Parameters.Add("@MaNV", SqlDbType.VarChar, 15).Value = cboNV.Text;
                mySqlCommand.Parameters.Add("@Quyen", SqlDbType.VarChar, 10).Value = txtQuyen.Text;

                mySqlCommand.Parameters.Add("@UserName1", SqlDbType.VarChar, 15).Value = User;
                mySqlCommand.Parameters.Add("@Password1", SqlDbType.VarChar, 15).Value = Pass;
                mySqlCommand.Parameters.Add("@MaNV1", SqlDbType.VarChar, 15).Value = MaNV;
                mySqlCommand.Parameters.Add("@Quyen1", SqlDbType.VarChar, 10).Value = Quyen;
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công");
            }
            //Truy van va hien thi lai du lieu tren luoi
            display("");
            SetControls(true);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUser.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtPass.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cboNV.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtQuyen.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }
    }
}
