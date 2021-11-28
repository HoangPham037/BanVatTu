using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace QuanLyCuaHangVatLieuXayDung
{
    internal class Database
    {
        SqlConnection mySqlConnection;
        private Database()
        {
            string conStr = @"Data Source=DESKTOP-MF0NP8H\SQLEXPRESS;Initial Catalog=CSDLQLBH;Integrated Security=True";
            this.mySqlConnection = new SqlConnection(conStr);

        }
    }
}
