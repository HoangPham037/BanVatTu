using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyCuaHangVatLieuXayDung
{
    internal class KhaiBao
    {
        string UserName = "", Password = "", MaNV = "", Quyen = "";

        private string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private bool isNew;
        public bool isExit = true;
        public event EventHandler Exit;
        DataTable tblNhanVien;
    }
}
