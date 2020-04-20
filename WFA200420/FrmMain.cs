using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA200420
{
    public partial class FrmMain : Form
    {
        SqlConnection conn;

        public FrmMain()
        {
            AppDomain.CurrentDomain.SetData(
                "DataDirectory",
                Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                .Location).Replace(@"bin\Debug", "Resources"));

            conn = new SqlConnection(
                @"Server=(localdb)\MSSQLLocalDB;" +
                @"AttachDbFileName=|DataDirectory|tripat.mdf;");

            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            DgvFeltolt(null);
            CbFeltolt();
        }

        private void CbFeltolt()
        {
            conn.Open();
            var cmd = new SqlCommand(
                "SELECT DISTINCT datum FROM kolcsonzes;", conn);

            var r = cmd.ExecuteReader();

            while (r.Read())
            {
                cbDatumok.Items.Add(r.GetDateTime(0).ToString("yyyy-MM-dd"));
            }
            conn.Close();
        }

        private void DgvFeltolt(string datumString)
        {
            dgv.Rows.Clear();

            conn.Open();
            var cmd = new SqlCommand(
                "SELECT datum, nev, nagy, szin FROM tricikli " +
                "INNER JOIN kolcsonzes ON tricId = jarmu " +
                "INNER JOIN patkany ON sofor = patkanyId" +
                ((datumString is null) ? ";" : $" WHERE datum = '{datumString}';"),
                conn);

            var r = cmd.ExecuteReader();
            while (r.Read())
            {
                dgv.Rows.Add(
                    r.GetDateTime(0).ToString("yyyy-MM-dd"),
                    r[1],
                    (r.GetBoolean(2) ? "nagy " : "kicsi ") + r[3]);
            }
            conn.Close();
        }

        private void cbDatumok_SelectedIndexChanged(object sender, EventArgs e)
        {
            DgvFeltolt(cbDatumok.SelectedItem.ToString());
        }

        private void obKep_Click(object sender, EventArgs e)
        {
            var frm = new FrmKolcsonzes(conn);
            frm.ShowDialog();
            cbDatumok.Text = null;
            DgvFeltolt(null);
        }
    }
}
