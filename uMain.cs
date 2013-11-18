using System;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;

namespace CTest1
{

    public partial class frmMain : Form
    {
        public FbConnection fbBD;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        public void frmMain_refrgrd(bool naritog)
        {
            esn_list aresn0 = new esn_list();

            aresn0.get_esn(212, Convert.ToDateTime("01/01/2013"), Convert.ToDateTime("31/12/2013"), naritog, true, false);
            esn_list aresn = aresn0.getone(aresn0[10].sotr_id);

            esn_list arorg = aresn.shrinklist(true);

            int i, j;
            for (i = 1; i < 13; i++)
            {
                bool bfind = true;
                for (j = 0; j < arorg.Count; j++)
                {
                    if (arorg[j].month_ == i)
                    {
                        bfind = false;
                    };
                };
                if (bfind)
                {
                    Esn_Class ecl = new Esn_Class();
                    ecl.year_ = 2013;
                    ecl.month_ = i;
                    ecl.Add("all_sum", 0);
                    ecl.Add("dmppf", 0);
                    ecl.Add("dnepf", 0);
                    ecl.Add("dbazapf", 0);
                    ecl.Add("dpf3", 0);
                    ecl.Add("dpf1", 0);
                    ecl.Add("dpf2", 0);
                    arorg.Add(ecl);
                };
            }


            arorg.Sort(delegate(Esn_Class ecl1, Esn_Class ecl2)
            { return ecl1.month_.CompareTo(ecl2.month_); });

            stg.RowCount = 12;
            stg.Columns[0].HeaderText = "ID";
            stg.Columns[1].HeaderText = @"sotrid";
            stg.Columns[2].HeaderText = @"god";
            stg.Columns[3].HeaderText = @"mes";
            stg.Columns[4].HeaderText = @"all";
            stg.Columns[5].HeaderText = @"mp";
            stg.Columns[6].HeaderText = @"ne";
            stg.Columns[7].HeaderText = @"baza";
            stg.Columns[8].HeaderText = @"pfstr";
            stg.Columns[9].HeaderText = @"pfnak";
            stg.Columns[10].HeaderText = @"pfprev";

            for (i = 0; i < 12; i++)
            {
                stg.Rows[i].Cells[0].Value = Convert.ToString(0);
                stg.Rows[i].Cells[1].Value = Convert.ToString(0);
                stg.Rows[i].Cells[2].Value = Convert.ToString(arorg[i].year_);
                stg.Rows[i].Cells[3].Value = Convert.ToString(arorg[i].month_);
                stg.Rows[i].Cells[4].Value = Convert.ToString(arorg[i].getstrvalue("all_sum"));
                stg.Rows[i].Cells[5].Value = Convert.ToString(arorg[i].getstrvalue("dmppf"));
                stg.Rows[i].Cells[6].Value = Convert.ToString(arorg[i].getstrvalue("dnepf"));
                stg.Rows[i].Cells[7].Value = Convert.ToString(arorg[i].getstrvalue("dbazapf"));
                stg.Rows[i].Cells[8].Value = Convert.ToString(arorg[i].getstrvalue("dpf2"));
                stg.Rows[i].Cells[9].Value = Convert.ToString(arorg[i].getstrvalue("dpf3"));
                stg.Rows[i].Cells[10].Value = Convert.ToString(arorg[i].getstrvalue("dpf1"));
            }

        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            frmMain_refrgrd(rbnarit.Checked);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmMain_refrgrd(rbnarit.Checked);
        }


        //Тест списков
        private void button4_Click(object sender, EventArgs e)
        {
            //Тест списка
            /*    esn_list lstvzn = new esn_list();

                esn_class ecl = new esn_class();
               ecl.id = 3;
                ecl.sotr_id=100233;
                ecl.Add("all_sum", 123);
                ecl.Add("pf", 123.34);
                ecl.Add("pf1", 123.44);
                ecl.Add("pf2", 123.4);
                ecl.Add("pf3", 12.33);
                lstvzn.Add(ecl);

                ecl = new esn_class();
                ecl.id = 3;
                ecl.sotr_id = 100233;
                ecl.Add("all_sum", 23657);
                ecl.Add("pf", 55.55);
                ecl.Add("pf1", 123.44);
                ecl.Add("pf2", 123.4);
                ecl.Add("pf3", 12.33);

                lstvzn.Add(ecl);

                 ecl = new esn_class();
                ecl.id = 3;
                ecl.sotr_id = 100233;
                ecl.Add("all_sum", 12233);
                ecl.Add("pf", 77.77);
                ecl.Add("pf1", 123.44);
                ecl.Add("pf2", 123.4);
                ecl.Add("pf3", 12.33);

                lstvzn.Add(ecl);

                int i = lstvzn.Count;

                for (i = 0; i < lstvzn.Count ; i++)
                {
                  lstvzn[i]["pf"] = i + 0.99;
                  Object s = lstvzn[i]["pf"];
                  double str = (double)s;
                  MessageBox.Show(Convert.ToString(str));
                } */


        }

        private void stg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMain_refrgrd(rbnarit.Checked);

        }



    }
}
