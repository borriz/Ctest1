using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace CTest1
{
    public static class RasPF
    {
        public static void rasesnall(int god, int mes, int org_id)
        {
            List<Int32> sotrlist = new List<Int32>();

            using (FbConnection fbBD = new FbConnection(FBDB.GetConnFBstring()))
            {
                fbBD.Open();
                using (FbCommand sqlreq = new FbCommand("select sotrid from sumnu where god_v=@g and mes_v<=@m group by sotrid ", fbBD))
                {
                    sqlreq.Parameters.AddWithValue("@g", Convert.ToString(god));
                    sqlreq.Parameters.AddWithValue("@m", Convert.ToString(mes));
                    FbDataReader r = sqlreq.ExecuteReader();
                    sotrlist.Clear();
                    while (r.Read())
                    {
                        sotrlist.Add(r.GetInt32(0));
                    };
                };
                fbBD.Close();
            };

            esn_list aresn = new esn_list();
            esn_list aresnpred = new esn_list();



            aresnpred.get_esn(212, new DateTime(god, mes, 1), new DateTime(god, mes, 31), true, true, false);
            aresn.build_esn(212, god, mes);

            int i;
            for (i = 0; i < sotrlist.Count; i++)
            {
                esn_list aresn1 = aresn.getone(sotrlist[i]);
                esn_list aresnpred1 = aresnpred.getone(sotrlist[i]);
                rasesnone(god, mes, sotrlist[i], aresn1, aresnpred1);

            };
        }

        public static void rasesnone(int god, int mes, int sotr_id, esn_list aresn, esn_list aresnpred)
        {
            //Определить дату рождения
            //Определить вычет по матпомощи
            //суммируем в 1
            //Ограничение
            //Расчет как 1
            //дозаполняем строки если есть null
            //Месячные суммы и распределение
            //Подгонка
            //Сохранение

        }
    }
}
