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
    public partial class frmLoaiHangHoa : Form
    {
        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private bool isNew;
        public event EventHandler Exit;
        DataTable tblLoaiHang;
        public frmLoaiHangHoa()
        {
            InitializeComponent();
        }

        private void frmLoaiHangHoa_Load(object sender, EventArgs e)
        {
            //kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            display("");
        }
        private void display(String TenLH)
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblLoaiHang  WHERE TenLH Like N'%" + TenLH + "%' ORDER by MaLH";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hien thi len luoi
            tblLoaiHang = new DataTable();
            tblLoaiHang.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = tblLoaiHang;
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtMaLH.Enabled = !edit;
            txtTenLH.Enabled = !edit;
            txtMieuTa.Enabled = !edit;
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
            txtMaLH.Clear();
            txtTenLH.Clear();
            txtMieuTa.Clear();
            //chuyen con tro ve txtFirstName
            txtTenLH.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtTenLH.Focus();
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
            string MaLH = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string TenLH = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string MieuTa = dataGridView1.Rows[row].Cells[2].Value.ToString();

            string sSql = "DELETE FROM tblLoaiHang WHERE (MaLH = @MaLH) and (TenLH = @TenLH) and (MieuTa = @MieuTa)";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = MaLH;
            mySqlCommand.Parameters.Add("@TenLH", SqlDbType.NVarChar, 30).Value = TenLH;
            mySqlCommand.Parameters.Add("@MieuTa", SqlDbType.NVarChar, 250).Value = MieuTa;
            mySqlCommand.ExecuteNonQuery();
            display("");

            MessageBox.Show("Xóa thành công");
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLH.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtTenLH.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtMieuTa.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtMaLH.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập loại hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaLH.Focus();
                return;
            }
            if (txtTenLH.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên loại hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenLH.Focus();
                return;
            }
            if (txtMieuTa.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập miêu tả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMieuTa.Focus();
                return;
            }
            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblLoaiHang (MaLH,TenLH,MieuTa) VALUES (@MaLH,@TenLH,@MieuTa)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = txtMaLH.Text;
                mySqlCommand.Parameters.Add("@TenLH", SqlDbType.NVarChar, 30).Value = txtTenLH.Text;
                mySqlCommand.Parameters.Add("@MieuTa", SqlDbType.NVarChar, 250).Value = txtMieuTa.Text;

                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string MaLH = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string TenLH = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string MieuTa = dataGridView1.Rows[row].Cells[2].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblLoaiHang SET MaLH = @MaLH, TenLH = @TenLH, MieuTa = @MieuTa WHERE (MaLH = @MaLH1) and (TenLH = @TenLH1) and (MieuTa = @MieuTa1)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = txtMaLH.Text;
                mySqlCommand.Parameters.Add("@TenLH", SqlDbType.NVarChar, 30).Value = txtTenLH.Text;
                mySqlCommand.Parameters.Add("@MieuTa", SqlDbType.NVarChar, 250).Value = txtMieuTa.Text;

                mySqlCommand.Parameters.Add("@MaLH1", SqlDbType.VarChar, 15).Value = MaLH;
                mySqlCommand.Parameters.Add("@TenLH1", SqlDbType.NVarChar, 30).Value = TenLH;
                mySqlCommand.Parameters.Add("@MieuTa1", SqlDbType.NVarChar, 250).Value = MieuTa;
                mySqlCommand.ExecuteNonQuery();
            }
            //Truy van va hien thi lai du lieu tren luoi
            display("");
            SetControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(true);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            display(txtTimKiem.Text);
        }
    }
}
