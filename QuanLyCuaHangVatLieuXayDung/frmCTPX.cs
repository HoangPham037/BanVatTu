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
    public partial class frmCTPX : Form
    {
        String sqlCon = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        SqlConnection SqlConnection = null;
        SqlCommand Command;
        DataTable dt;
        private bool isNew;
        private string _shd;
        private string _ngaylap;
        private string _tenkh;
        private string _tennv;
        private string _diachi;
        private string _sdt;
        public frmCTPX()
        {
            InitializeComponent();
        }
        public String SHD
        {
            get { return _shd; }
            set { _shd = value; }
        }
        public String NgayLap
        {
            get { return _ngaylap; }
            set { _ngaylap = value; }
        }
        public String TenKH
        {
            get { return _tenkh; }
            set { _tenkh = value; }
        }
        public String TenNV
        {
            get { return _tennv; }
            set { _tennv = value; }
        }
        public String SDT
        {
            get { return _sdt; }
            set { _sdt = value; }
        }

        private void frmCTPX_Load(object sender, EventArgs e)
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
            LoadComboBoxHH();
            LoadComboBoxKho();


            lbSHD.Text = _shd;
            lbNgayLap.Text = _ngaylap;
            lbKhachHang.Text = _tenkh;
            lbNV.Text = _tennv;
            lbSDT.Text = _sdt;

        }
        private void display()
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblChiTietHDXuat";
            Command = new SqlCommand(sSql, SqlConnection);
            SqlDataReader mySqlDataReader = Command.ExecuteReader();

            //Hien thi len luoi
            DataTable dt = new DataTable();
            dt.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //thiet lap cac trang thai
            isNew = true;
            SetControls(false);
            //xoa trang cac textboxes
            txtDonGia.Clear();
            txtSoLuong.Clear();
            txtTongTien.Clear();
            //chuyen con tro ve txtFirstName
            txtDonGia.Focus();
        }
        void LoadComboBoxHH()
        {
            Command = new SqlCommand("select * from tblHangHoa", SqlConnection);
            SqlDataReader sqlDataReader = Command.ExecuteReader();
            dt = new DataTable();
            dt.Load(sqlDataReader);
            cboHangHoa.DisplayMember = "TenHH";
            cboHangHoa.ValueMember = "DonViTinh";
            cboHangHoa.DataSource = dt;
        }
        void LoadComboBoxKho()
        {
            Command = new SqlCommand("select * from tblKho", SqlConnection);
            SqlDataReader sqlDataReader = Command.ExecuteReader();
            dt = new DataTable();
            dt.Load(sqlDataReader);
            cboKho.DisplayMember = "IDKho";
            cboKho.DataSource = dt;
        }
        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtSoLuong.Enabled = !edit;
            txtDonGia.Enabled = !edit;
            cboHangHoa.Enabled = !edit;
            txtDonVi.Enabled = !edit;
            txtTongTien.Enabled = !edit;
            //thiet lap trang thai Enable/Disable cho cac nut an
            btnAdd.Enabled = edit;
            btnEdit.Enabled = edit;
            btnDelete.Enabled = edit;
            btnSave.Enabled = !edit;
            btnCancel.Enabled = !edit;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtDonGia.Focus();
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
            string IDKho = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string TenHH = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string MaHDX = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string SoLuongXuat = dataGridView1.Rows[row].Cells[4].Value.ToString();
            string DonGiaXuat = dataGridView1.Rows[row].Cells[5].Value.ToString();


            string sSql = "DELETE FROM tblChiTietHDXuat WHERE (IDKho = @IDKho) and (TenHH = @TenHH) and (SoHDXuat = @SoHDXuat) and (DonGiaXuat = @DonGiaXuat) " +
                    "and (SoLuongXuat = @SoLuongXuat)";
            Command = new SqlCommand(sSql, SqlConnection);
            Command.Parameters.Add("@IDKho", SqlDbType.Int).Value = IDKho;
            Command.Parameters.Add("@TenHH",SqlDbType.NVarChar, 50).Value = TenHH;
            Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = MaHDX;
            Command.Parameters.Add("@SoLuongXuat", SqlDbType.Int).Value = SoLuongXuat;
            Command.Parameters.Add("@DonGiaXuat", SqlDbType.Int).Value = DonGiaXuat;

            Command.ExecuteNonQuery();
            display();
            MessageBox.Show("Xóa thành công");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDonGia.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập đơn giá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDonGia.Focus();
                return;
            }
            if (txtSoLuong.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Focus();
                return;
            }

            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblChiTietHDXuat (IDKho,TenHH,SoHDXuat,SoLuongXuat,DonGiaXuat,TongTien) VALUES (@IDKho,@TenHH,@SoHDXuat,@SoLuongXuat,@DonGiaXuat,@TongTien)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@IDKho", SqlDbType.Int).Value = cboKho.Text.Trim();
                Command.Parameters.Add("@TenHH",SqlDbType.NVarChar, 50).Value = cboHangHoa.Text.Trim();
                Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = lbSHD.Text;
                Command.Parameters.Add("@SoLuongXuat", SqlDbType.Int).Value = txtSoLuong.Text;
                Command.Parameters.Add("@DonGiaXuat", SqlDbType.Int).Value = txtDonGia.Text;
                Command.Parameters.Add("@DTongTien", SqlDbType.Int).Value = txtDonGia.Text;
                int sl = Convert.ToInt32(txtSoLuong.Text);
                int dongia = Convert.ToInt32(txtDonGia.Text);
                txtTongTien.Text = Convert.ToString(sl * dongia);
                Command.Parameters.Add("@TongTien", SqlDbType.Int).Value = txtTongTien.Text;
                Command.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string IDKho = dataGridView1.Rows[row].Cells[1].ToString();
                string TenHH = dataGridView1.Rows[row].Cells[2].ToString();
                string SoHDN = dataGridView1.Rows[row].Cells[3].Value.ToString();
                string SoLuongNhap = dataGridView1.Rows[row].Cells[4].Value.ToString();
                string DonGiaNhap = dataGridView1.Rows[row].Cells[5].Value.ToString();
                //Update
                //dung tham so
                string sSql = "UPDATE tblChiTietHDN SET IDKho = @IDKho, SoHDXuat = @SoHDXuat, SoLuongXuat = @SoLuongXUat, DonGiaXuat = @DonGiaXuat" +
                              "WHERE (IDKho = @IDKho1) and (SoHDXuat = @SoHDXuat1) and (SoLuongXuat = @SoLuongXuat1) and (DonGiaXuat = @DonGiaXuat1)";
                Command = new SqlCommand(sSql, SqlConnection);
                Command.Parameters.Add("@IDKho", SqlDbType.Int).Value = cboKho.Text;
                Command.Parameters.Add("@SoHDXuat", SqlDbType.VarChar, 15).Value = lbSHD.Text;
                Command.Parameters.Add("@SoLuongXuat", SqlDbType.Int).Value = txtSoLuong.Text;
                Command.Parameters.Add("@DonGiaXuat", SqlDbType.Int).Value = txtDonGia.Text;

                Command.Parameters.Add("@IDKho1", SqlDbType.Int).Value = IDKho;
                Command.Parameters.Add("@SoHDXuat1", SqlDbType.VarChar, 15).Value = SoHDN;
                Command.Parameters.Add("@SoLuongNhap1", SqlDbType.Int).Value = SoLuongNhap;
                Command.Parameters.Add("@DonGiaXuat1", SqlDbType.Int).Value = DonGiaNhap;

                Command.ExecuteNonQuery();
                MessageBox.Show("Sửa thành công!");
            }
            //Truy van va hien thi lai du lieu tren luoi
            display();
            SetControls(true);
           
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cboKho.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cboHangHoa.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            lbSHD.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSoLuong.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtDonGia.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtTongTien.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void cboHangHoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDonVi.Text = cboHangHoa.SelectedValue.ToString();
        }
    }
}
