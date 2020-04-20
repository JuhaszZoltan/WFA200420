using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA200420
{
    public partial class FrmKolcsonzes : Form
    {
        private SqlConnection conn;
        public FrmKolcsonzes(SqlConnection conn)
        {
            this.conn = conn;
            InitializeComponent();
        }

        private void FrmKolcsonzes_Load(object sender, EventArgs e)
        {
            tbDatum.Text = DateTime.Today.ToString("yyyy-MM-dd");
            CbFeltol();
            DgvFeltolt();
        }

        private void DgvFeltolt()
        {
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM tricikli;", conn);

            var r = cmd.ExecuteReader();

            while (r.Read())
            {
                dgv.Rows.Add(r[0], r.GetBoolean(1) ? "nagy" : "kicsi", r[2]);
            }
            conn.Close();
        }

        private void CbFeltol()
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT nev FROM patkany;", conn);
            var r = cmd.ExecuteReader();

            while (r.Read())
            {
                cbNevek.Items.Add(r["nev"]);
            }
            conn.Close();
        }

        private void btnUj_Click(object sender, EventArgs e)
        {
            if (cbNevek.SelectedIndex == -1)
            {
                MessageBox.Show("Válassz ki egy patkányt!");
            }
            else if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Válassz triciklit is!");
            }
            else
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "SELECT alfaj FROM patkany " +
                    $"WHERE patkanyId = {cbNevek.SelectedIndex + 1};",
                    conn);

                var r = cmd.ExecuteReader();
                r.Read();

                var res = DialogResult.Yes;

                if (r.GetString(0) == "cricetomys")
                {
                    res = MessageBox.Show(
                        "Egy NAGY patkány KICSI triciklit próbál kölcsönözni.\nBiztosan folytatod?",
                        "FIGYELMEZTETÉS!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                }

                r.Close();
                if (res == DialogResult.Yes)
                {
                    var adp = new SqlDataAdapter()
                    {
                        InsertCommand = new SqlCommand(
                            "INSERT INTO kolcsonzes (datum, sofor, jarmu) VALUES " +
                            $"('{tbDatum.Text}', {cbNevek.SelectedIndex + 1}, {dgv.SelectedRows[0].Cells[0].Value});", conn),
                    };
                    adp.InsertCommand.ExecuteNonQuery();
                }
                conn.Close();

                this.Dispose();
            }
        }
    }
}
