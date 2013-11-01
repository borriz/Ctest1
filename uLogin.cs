using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Runtime.InteropServices;

using System.Text;
using System.Windows.Forms;


using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;





namespace CTest1
{
    public enum CRYPT_STRING_FLAGS : uint
    {
        CRYPT_STRING_BASE64HEADER = 0,
        CRYPT_STRING_BASE64 = 1,
        CRYPT_STRING_BINARY = 2,
        CRYPT_STRING_BASE64REQUESTHEADER = 3,
        CRYPT_STRING_HEX = 4,
        CRYPT_STRING_HEXASCII = 5,
        CRYPT_STRING_BASE64_ANY = 6,
        CRYPT_STRING_ANY = 7,
        CRYPT_STRING_HEX_ANY = 8,
        CRYPT_STRING_BASE64X509CRLHEADER = 9,
        CRYPT_STRING_HEXADDR = 10,
        CRYPT_STRING_HEXASCIIADDR = 11,
        CRYPT_STRING_HEXRAW = 12,
        CRYPT_STRING_NOCRLF = 0x40000000,
        CRYPT_STRING_NOCR = 0x80000000
    }

    public partial class frmLogin : Form
    {
        [DllImport("crypt32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CryptStringToBinary(string sPEM, UInt32 sPEMLength, CRYPT_STRING_FLAGS dwFlags, [Out] byte[] pbBinary, ref UInt32 pcbBinary, out UInt32 pdwSkip, out UInt32 pdwFlags);

        public string Login, Pass;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            tbLogin.Text = "sysadmin";
            mtbPass.Text = "daria";

        }

        private string decrypt_pwd(string s)
        {
            byte[] r ;
            UInt32 sz = new  UInt32();
            UInt32 skip = new  UInt32();
            UInt32 flags = (UInt32)CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64;
            UInt32 s1 = (UInt32)s.Length;

            CryptStringToBinary(s, s1, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64, null, ref sz, out skip, out flags);
            r= new byte[sz];
            CryptStringToBinary(s, s1, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64, r, ref sz, out skip, out flags);

            string str = Encoding.ASCII.GetString(r);

            return str;
        }

  

        private void btnOk_Click(object sender, EventArgs e)
        {
            string Login = tbLogin.Text;
            string Pass = mtbPass.Text;
            bool passOk;

            using (FbConnection fbBD = new FbConnection(FBDB.GetConnFBstring()))
            {
                if (fbBD.State == ConnectionState.Closed)
                    fbBD.Open();
                FbCommand sqlReqest = new FbCommand("select u.pswd,u.typ_pswd from USERS u where u.logname='" + Login + "'", fbBD);
                FbDataReader r = sqlReqest.ExecuteReader();


                if (r.Read())
                {
                    string PassBD = decrypt_pwd(r.GetString(0));

                    if (Pass == PassBD)
                    {
                        passOk = true;
                    }
                    else
                    {
                        passOk = false;
                    };
                }
                else
                {
                    passOk = false;
                };
                r.Close();
                sqlReqest.Dispose();
                fbBD.Close();
            }
            if (passOk)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином/паролем не найден");
            };

        }
    }
}
