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
    public partial class frmHangHoa : Form
    {
        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        
        private SqlConnection mySqlConnection;
        
        private SqlCommand mySqlCommand;
        private bool isNew;
        public bool isExit = true;
        public event EventHandler Exit;
        DataTable tblHangHoa;
        public frmHangHoa()
        {
            InitializeComponent();
        }

        private void frmHangHoa_Load(object sender, EventArgs e)
        {


            //kết nối tới CSDL
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            LoadComboBox();

            display("");

        }
        void LoadComboBox()
        {
            mySqlCommand = new SqlCommand("select MaLH from tblLoaiHang", mySqlConnection);
            SqlDataReader sqlDataReader = mySqlCommand.ExecuteReader();
            tblHangHoa = new DataTable();
            tblHangHoa.Load(sqlDataReader);
            cbLoaiHang.DisplayMember = "MaLH";
            cbLoaiHang.DataSource = tblHangHoa;


        }
        private void display(String TenHH)
        {
            //Truy vấn dữ liệu
            string sSql = "SELECT * FROM tblHangHoa  WHERE TenHH Like N'%" + TenHH + "%' ORDER by MaHH";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            //Hien thi len luoi
            tblHangHoa = new DataTable();
            tblHangHoa.Load(mySqlDataReader); //chuyển từ DataReader sang DataTable
            dataGridView1.DataSource = tblHangHoa;
        }

        private void SetControls(bool edit)
        {
            //thiet lap trang thai Enable/Disable cho cac textbox
            txtMaHH.Enabled = !edit;
            cbLoaiHang.Enabled = !edit;
            txtTenHH.Enabled = !edit;
            txtDonViTinh.Enabled = !edit;
            txtXuatXu.Enabled = !edit;
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
            txtMaHH.Clear();
            txtTenHH.Clear();
            txtDonViTinh.Clear();
            txtXuatXu.Clear();
            //chuyen con tro ve txtFirstName
            txtMaHH.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //chuyen con tro ve txtFirstName
            txtMaHH.Focus();
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
            string MaHH = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string TenHH = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string DonViTinh = dataGridView1.Rows[row].Cells[3].Value.ToString();
            string XuatXu = dataGridView1.Rows[row].Cells[4].Value.ToString();

            string sSql = "DELETE FROM tblHangHoa WHERE (MaLH = @MaLH) and (MaHH = @MaHH) and (TenHH = @TenHH) and (DonViTinh = @DonViTinh) and (XuatXu = @XuatXu) ";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = MaLH;
            mySqlCommand.Parameters.Add("@MaHH", SqlDbType.NVarChar, 15).Value = MaHH;
            mySqlCommand.Parameters.Add("@TenHH", SqlDbType.NVarChar, 50).Value = TenHH;
            mySqlCommand.Parameters.Add("@DonViTinh", SqlDbType.NVarChar, 50).Value = DonViTinh;
            mySqlCommand.Parameters.Add("@XuatXu", SqlDbType.NVarChar, 50).Value = XuatXu;
            mySqlCommand.ExecuteNonQuery();
            display("");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtMaHH.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập mã hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaHH.Focus();
                return;
            }
            if (txtTenHH.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên hàng hóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenHH.Focus();
                return;
            }
            if (txtDonViTinh.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập đơn vị tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDonViTinh.Focus();
                return;
            }
            if (txtXuatXu.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập xuất xứ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtXuatXu.Focus();
                return;
            }

            if (isNew)
            {
                //dung tham so
                string sSql = "INSERT INTO tblHangHoa (MaLH,MaHH,TenHH,DonViTinh,XuatXu) VALUES (@MaLH,@MaHH,@TenHH,@DonViTinh,@XuatXu)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);

                mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = cbLoaiHang.Text;
                mySqlCommand.Parameters.Add("@MaHH", SqlDbType.VarChar, 15).Value = txtMaHH.Text;
                mySqlCommand.Parameters.Add("@TenHH", SqlDbType.NVarChar, 50).Value = txtTenHH.Text;
                mySqlCommand.Parameters.Add("@DonViTinh", SqlDbType.NVarChar, 50).Value = txtDonViTinh.Text;
                mySqlCommand.Parameters.Add("@XuatXu", SqlDbType.NVarChar, 00).Value = txtXuatXu.Text;


                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //sua du lieu
                //Lay du lieu tren luoi
                int row = dataGridView1.CurrentRow.Index;
                string MaLH = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string MaHH = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string TenHH = dataGridView1.Rows[row].Cells[2].Value.ToString();
                string DonViTinh = dataGridView1.Rows[row].Cells[3].Value.ToString();
                string XuatXu = dataGridView1.Rows[row].Cells[4].Value.ToString();

                //Update
                //dung tham so
                string sSql = "UPDATE tblHangHoa SET MaLH=@MaLH, MaHH = @MaHH, TenHH = @TenHH, DonViTinh = @DonViTinh" +
                    ", XuatXu = @XuatXu WHERE (MaLH=@MaLH1) and (MaHH = @MaHH1) and(TenHH = @TenHH1) and (DonViTinh = @DonViTinh1) and (XuatXu = @XuatXu1)";
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.Parameters.Add("@MaLH", SqlDbType.VarChar, 15).Value = cbLoaiHang.Text;
                mySqlCommand.Parameters.Add("@MaHH", SqlDbType.VarChar, 15).Value = txtMaHH.Text;
                mySqlCommand.Parameters.Add("@TenHH", SqlDbType.NVarChar, 50).Value = txtTenHH.Text;
                mySqlCommand.Parameters.Add("@DonViTinh", SqlDbType.NVarChar, 50).Value = txtDonViTinh.Text;
                mySqlCommand.Parameters.Add("@XuatXu", SqlDbType.NVarChar, 50).Value = txtXuatXu.Text;;

                mySqlCommand.Parameters.Add("@MaLH1", SqlDbType.VarChar, 15).Value = MaLH;
                mySqlCommand.Parameters.Add("@MaHH1", SqlDbType.NVarChar, 15).Value = MaHH;
                mySqlCommand.Parameters.Add("@TenHH1", SqlDbType.NVarChar, 50).Value = TenHH;
                mySqlCommand.Parameters.Add("@DonViTinh1", SqlDbType.NVarChar, 50).Value = DonViTinh;
                mySqlCommand.Parameters.Add("@XuatXu1", SqlDbType.NVarChar, 50).Value = XuatXu;
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

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cbLoaiHang.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtMaHH.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtTenHH.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtDonViTinh.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtXuatXu.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            display(txtTimKiem.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
