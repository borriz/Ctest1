using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace CTest1
{
    class Esn_Class : Dictionary<string, double>
    {
        public int id;
        public int sotr_id;
        public int dts_id;
        public int org_id;
        public int tipsotr;
        public int squadid;
        public int year_;
        public int month_;
        public int tag;

        public int schet;
        public int def_prov;
        public int prov921;
        public int prov922;
        public int prov923;
        public int prov901;
        public int prov911;
        public int prov903;
        public int prov904;
        public int prov924;
        public int sprrash_id;


        public void zapolnesn(bool notsumpfspf2, FbDataReader r, int god, int mes)
        {
            this.id = r.GetInt32(35);
            this.sotr_id = r.GetInt32(14);
            this.dts_id = r.GetInt32(42);
            this.org_id = r.GetInt32(15);
            this.tipsotr = r.GetInt32(32);
            this.squadid = r.GetInt32(26);
            this.year_ = god;
            this.month_ = mes;
            this.tag = r.GetInt32(37);

            this.schet = r.GetInt32(17);
            this.def_prov = r.GetInt32(18);
            this.prov921 = r.GetInt32(19);
            this.prov922 = r.GetInt32(21);
            this.prov923 = r.GetInt32(20);
            this.prov901 = r.GetInt32(22);
            this.prov911 = r.GetInt32(23);
            this.prov903 = r.GetInt32(24);
            this.prov904 = r.GetInt32(25);
            this.prov924 = r.GetInt32(34);
            this.sprrash_id = r.GetInt32(30);

            //Плавающие поля
            this.Add("all_sum", r.GetDouble(0));
            this.Add("dmppf", r.GetDouble(4));
            this.Add("dnepf", r.GetDouble(27));
            this.Add("dbazapf", r.GetDouble(1));
            this.Add("dpf3", r.GetDouble(13));
            if (notsumpfspf2)
            {
                this.Add("dpf1", r.GetDouble(38));
                this.Add("dpf2", r.GetDouble(12));
            }
            else
            {
                this.Add("dpf2", r.GetDouble(38) + r.GetDouble(12));
                this.Add("dpf1", 0);
            };

        }

    }



    class esn_list : List<Esn_Class>
    {

        public esn_list()
        {
            //Конструктор
            this.Clear();
        }

        public void get_esn(int org_id, DateTime db, DateTime de, bool naritog, bool notsumpfspf2, bool onlylockinmonth)
        {

            string qstr, period;
            int wmonths = db.Month;
            int wmonthe = de.Month;
            int god = db.Year;


            if (onlylockinmonth)
            {
                period = " e.year_=@year_ AND ((e.month_ BETWEEN @month_s AND @month_e1) or (e.month_=@month_e and e.lck=1)) ";
            }
            else
            {
                period = " e.year_=@year_ AND e.month_ BETWEEN @month_s AND @month_e ";
            };


            using (FbConnection fbBD = new FbConnection(FBDB.GetConnFBstring()))
            {
                fbBD.Open();
                if (naritog)
                {
                    qstr = "SELECT sum(e.all_sum) as all_sum, sum(e.baza_pf) as baza_pf,sum(e.baza_foms) as baza_foms,sum(e.baza_fss_osn) as baza_fss_osn, " //3
                + " sum(e.mp_pf) as mp_pf,  sum(e.mp_fssosn) as mp_fssosn, sum(e.mp_foms) as mp_foms, " //6
                + " sum(e.baza_fss_pr) as baza_fss_pr,sum(e.fss_pr) as fss_pr,sum(e.fss_osn) as fss_osn, " //9
                + " sum(e.ffoms) as ffoms,sum(e.tfoms) as tfoms,sum(e.pf2) as pf2, " //12
                + " sum(e.pf3) as pf3, e.sotr_id,e.org_id, sum(e.rash_ss) as rash_ss,e.schet, " //17
                + " e.defprov, e.prov921,e.prov923,e.prov922,e.prov901,e.prov911,e.prov903,e.prov904,e.squadid, " //26
                + " sum(e.ne_pf) as ne_pf,  sum(e.ne_fssosn) as ne_fssosn, sum(e.ne_foms) as ne_foms, e.SPRRASH_ID,0,e.tipsotr, " //32
                + " sum(e.pf_dopsotr) as pf_dopsotr,e.prov924,0,sum(e.mp_fsstr), e.tag ,sum(e.pf1) as pf1,sum(e.baza_rv3) as baza_rv3, " // //39
                + " sum(e.pf_rv3) as pf_rv3, 0, e.dts_id, sum(e.pf1_dopart)" //50
                + " FROM esn e "
                + " WHERE not e.all_sum is null and " + period;

                    qstr = qstr + "  and e.org_id = " + org_id.ToString();
                    qstr = qstr + " group by e.sotr_id,e.org_id,e.schet, e.tag, " +
                " e.defprov, e.prov921,e.prov923,e.prov922,e.prov901,e.prov911,e.prov903,e.prov904,e.squadid,e.SPRRASH_ID,e.prov924,e.tipsotr, e.dts_id";


                    this.Clear();
                    int t;
                    for (t = wmonths; t <= wmonthe; t++)
                    {

                        using (FbCommand sqlReq = new FbCommand(qstr, fbBD))
                        {
                            sqlReq.Parameters.AddWithValue("@month_s", 1);
                            sqlReq.Parameters.AddWithValue("@month_e", t);
                            sqlReq.Parameters.AddWithValue("@year_", god);
                            if (onlylockinmonth)
                            {
                                sqlReq.Parameters.AddWithValue("@month_e1", t);
                            };

                            FbDataReader r = sqlReq.ExecuteReader();
                            ////////////

                            while (r.Read())
                            {
                                Esn_Class ecl = new Esn_Class();
                                ecl.zapolnesn(notsumpfspf2, r, god, t);
                                this.Add(ecl);
                            };
                            r.Close();
                            r.Dispose();
                        }
                        //////////////////
                    }
                }
                else
                {
                    qstr = "SELECT e.all_sum, e.baza_pf,e.baza_foms,e.baza_fss_osn, "
                 + " e.mp_pf,  e.mp_fssosn, e.mp_foms, "
                 + " e.baza_fss_pr,e.fss_pr,e.fss_osn, "
                 + " e.ffoms,e.tfoms,e.pf2, e.pf3, "
                 + " e.sotr_id,e.org_id, e.rash_ss,e.schet, "
                 + " e.defprov, e.prov921,e.prov923,e.prov922,e.prov901,e.prov911,e.prov903,e.prov904,e.squadid, e.ne_pf,e.ne_fssosn,e.ne_foms, e.SPRRASH_ID,"
                 + " e.month_,e.tipsotr, e.pf_dopsotr,e.prov924,e.id,e.mp_fsstr, e.tag, e.pf1, e.baza_rv3,e.pf_rv3, e.lck, e.dts_id, e.pf1_dopart " //51
                 + " FROM esn e  "
                 + " WHERE not e.all_sum is null and " + period;
                    qstr = qstr + "  and e.org_id = " + org_id.ToString();

                    using (FbCommand sqlReq = new FbCommand(qstr, fbBD))
                    {
                        sqlReq.Parameters.AddWithValue("@month_s", wmonths);
                        sqlReq.Parameters.AddWithValue("@month_e", wmonthe);
                        sqlReq.Parameters.AddWithValue("@year_", god);
                        if (onlylockinmonth)
                        {
                            sqlReq.Parameters.AddWithValue("@month_e1", wmonthe);
                        };

                        FbDataReader r = sqlReq.ExecuteReader();
                        ////////////
                        this.Clear();
                        while (r.Read())
                        {
                            Esn_Class ecl = new Esn_Class();
                            ecl.zapolnesn(notsumpfspf2, r, god, r.GetInt32(31));
                            this.Add(ecl);
                        };
                        r.Close();
                        r.Dispose();
                    }

                };
                fbBD.Close();
            }



            //   MessageBox.Show(Convert.ToString(this.Count));
        }

        public esn_list shrinklist(bool bezsotr)
        {
            esn_list rez = new esn_list();
            int i, j;
            bool find;
            for (i = 0; i < this.Count; i++)
            {
                find = false;
                for (j = 0; j < rez.Count; j++)
                {
                    if ((this[i].month_ == rez[j].month_) && (this[i].sotr_id == rez[j].sotr_id || bezsotr))
                    {
                        foreach (string s in this[i].Keys)
                        {
                            rez[j][s] = rez[j][s] + this[i][s];
                        }
                        find = true;
                    }
                }

                if (!find)
                {
                    Esn_Class ecl = new Esn_Class();
                    ecl.id = 0;
                    ecl.sotr_id = this[i].sotr_id;
                    ecl.dts_id = this[i].dts_id;
                    ecl.org_id = this[i].org_id;
                    ecl.tipsotr = this[i].tipsotr;
                    ecl.squadid = this[i].squadid;
                    ecl.year_ = this[i].year_;
                    ecl.month_ = this[i].month_;
                    ecl.tag = this[i].tag;

                    ecl.schet = this[i].schet;
                    ecl.def_prov = this[i].def_prov;
                    ecl.prov921 = this[i].prov921;
                    ecl.prov922 = this[i].prov922;
                    ecl.prov923 = this[i].prov923;
                    ecl.prov901 = this[i].prov901;
                    ecl.prov911 = this[i].prov911;
                    ecl.prov903 = this[i].prov903;
                    ecl.prov904 = this[i].prov904;
                    ecl.prov924 = this[i].prov924;
                    ecl.sprrash_id = this[i].sprrash_id;

                    //Плавающие поля
                    foreach (string s in this[i].Keys)
                    {
                        ecl.Add(s, this[i][s]);

                    }
                    rez.Add(ecl);
                    find = true;

                }



            }





            return rez;
        }


        public void Zap_arESNall(int tip, int org_id, int imonth, int iyear, FbDataReader r)
        {
            bool find = false;
            int j;
            while (r.Read())
            {
                for (j = 0; j < this.Count; j++)
                {
                    if ((this[j].sotr_id == r.GetInt32(0)) &&
                        (this[j].schet == r.GetInt32(3)) &&
                        (this[j].def_prov == r.GetInt32(2)) &&
                        (this[j].sprrash_id == r.GetInt32(4)) &&
                        (this[j].squadid == r.GetInt32(5)) &&
                        (this[j].dts_id == r.GetInt32(8)))
                    {
                        switch (tip)
                        {
                            case 0:
                                if (this[j].ContainsKey("all_sum"))
                                { this[j]["all_sum"] = this[j]["all_sum"] + r.GetDouble(1); }
                                else { this[j].Add("all_sum", r.GetDouble(1)); };
                                break;
                            case 1:
                                if (this[j].ContainsKey("dbazapf"))
                                { this[j]["dbazapf"] = this[j]["dbazapf"] + r.GetDouble(1); }
                                else { this[j].Add("dbazapf", r.GetDouble(1)); };
                                break;
                            case 2:
                                if (this[j].ContainsKey("dmppf"))
                                { this[j]["dmppf"] = this[j]["dmppf"] + r.GetDouble(1); }
                                else { this[j].Add("dmppf", r.GetDouble(1)); };
                                break;
                        }
                        find = true;
                    }
                }

                if (!find)
                {
                    Esn_Class ecl = new Esn_Class();
                    ecl.id = 0;
                    ecl.sotr_id = r.GetInt32(0);
                    ecl.dts_id = r.GetInt32(8);
                    ecl.org_id = org_id;
                    ecl.squadid = r.GetInt32(5);
                    ecl.year_ = iyear;
                    ecl.month_ = imonth;
                    ecl.schet = r.GetInt32(3);
                    ecl.def_prov = r.GetInt32(2);
                    ecl.sprrash_id = r.GetInt32(4);

                    ecl.tag = 0;
                    ecl.tipsotr = 0;
                    //Плавающие поля
                    switch (tip)
                    {
                        case 0:
                            ecl.Add("all_sum", r.GetDouble(1));
                            break;
                        case 1:
                            ecl.Add("dbazapf", r.GetDouble(1));
                            break;
                        case 2:
                            ecl.Add("dmppf", r.GetDouble(1));
                            break;
                    }
                    ecl.Add("dnepf", 0);
                    ecl.Add("dpf1", 0);
                    ecl.Add("dpf2", 0);
                    ecl.Add("dpf3", 0);


                    this.Add(ecl);
                    find = true;

                }


            };

        }


        public void build_esn(int org_id, int god, int mes)
        {
            this.Clear();
            using (FbConnection fbBD = new FbConnection(FBDB.GetConnFBstring()))
            {
                fbBD.Open();
                //Все суммы
                string ssql = " SELECT s.sotrid,SUM(s.summa),s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id , s.god_v, s.mes_v,s.dts_id "
               + " FROM sumnu s,nachisl n,sotrudn so, dlg_to_struct dd "
               + " WHERE s.mes_v<=@mes_v AND s.god_v=@god_v AND s.nu_id=n.n_id and dd.unique_id=s.dts_id and dd.squad_id in (select newstruct.st_uid from newstruct where newstruct.st_org=@org) "
               + " AND NOT n.privid IN (58,581,1001,-98,-99,-50) AND n.u_n='+' AND n.is_virtual='F'  AND s.sotrid=so.sotrudn_id "
               + " GROUP BY s.sotrid,s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id, s.god_v, s.mes_v ,s.dts_id "
               + " HAVING SUM(s.summa) IS NOT NULL and SUM(s.summa)<>0 "
               + " ORDER BY s.sotrid ";
                using (FbCommand sqlReq = new FbCommand(ssql, fbBD))
                {
                    sqlReq.Parameters.AddWithValue("@mes_v", mes);
                    sqlReq.Parameters.AddWithValue("@god_v", god);
                    sqlReq.Parameters.AddWithValue("@org", org_id);
                    FbDataReader r = sqlReq.ExecuteReader();

                    Zap_arESNall(0, org_id, god, mes, r);
                    r.Close();
                    r.Dispose();

                    // База ПФ
                    ssql = " SELECT s.sotrid,SUM(s.summa),s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id , s.god_v, s.mes_v,s.dts_id "
                      + " FROM sumnu s,nachisl n,sotrudn so, dlg_to_struct dd "
                      + " WHERE s.mes_v<=@mes_v AND s.god_v=@god_v AND s.nu_id=n.n_id and dd.unique_id=s.dts_id and dd.squad_id in (select newstruct.st_uid from newstruct where newstruct.st_org=@org) "
                      + " AND NOT n.privid IN (58,581,1001,-98,-99,-50) AND n.u_n='+' AND n.is_virtual='F' AND s.sotrid=so.sotrudn_id "
                      + " AND n.fp='T'  "
                      + " GROUP BY s.sotrid,s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id, s.god_v, s.mes_v ,s.dts_id "
                      + " HAVING SUM(s.summa) IS NOT NULL and SUM(s.summa)<>0 "
                      + " ORDER BY s.sotrid ";
                    sqlReq.CommandText = ssql;
                    sqlReq.Parameters.AddWithValue("@mes_v", mes);
                    sqlReq.Parameters.AddWithValue("@god_v", god);
                    sqlReq.Parameters.AddWithValue("@org", org_id);
                    r = sqlReq.ExecuteReader();
                    Zap_arESNall(1, org_id, god, mes, r);
                    r.Close();
                    r.Dispose();
                    //Матпомощь ПФ

                    ssql = " SELECT s.sotrid,SUM(s.summa),s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id , s.god_v, s.mes_v,s.dts_id "
                      + " FROM sumnu s,nachisl n,sotrudn so, dlg_to_struct dd "
                      + " WHERE s.mes_v<=@mes_v AND s.god_v=@god_v AND s.nu_id=n.n_id and dd.unique_id=s.dts_id AND n.fp='T' and dd.squad_id in (select newstruct.st_uid from newstruct where newstruct.st_org=@org) "
                      + " AND(n.privid  in (19,20,21,22,55,306)) AND NOT n.privid IN (-98,-99,-50) AND "
                      + " s.sotrid=so.sotrudn_id "
                      + " GROUP BY s.sotrid,s.esn_prov,s.schet,s.EX_SPRRASH_ID , dd.squad_id, s.god_v, s.mes_v ,s.dts_id "
                      + " HAVING SUM(s.summa) IS NOT NULL ";
                    sqlReq.CommandText = ssql;
                    sqlReq.Parameters.AddWithValue("@mes_v", mes);
                    sqlReq.Parameters.AddWithValue("@god_v", god);
                    sqlReq.Parameters.AddWithValue("@org", org_id);
                    r = sqlReq.ExecuteReader();
                    Zap_arESNall(2, org_id, god, mes, r);
                    r.Close();
                    r.Dispose();
                }
                fbBD.Close();
            }

        }

        //Конец класса Esn_List
    }

}
