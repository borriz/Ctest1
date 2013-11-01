using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CTest1
{
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var flogin = new frmLogin();
            if (flogin.ShowDialog() == DialogResult.OK)
            {
                //Проверка логина
               
                    Application.Run(new frmMain());
         

            };
           
            
        }
    }
}
