using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace CTest1
{
    public static class FBDB
    {
        public static string GetConnFBstring()
        { 
             FbConnectionStringBuilder csb = new FbConnectionStringBuilder();

            // Указываем тип используемого сервера
            csb.ServerType = FbServerType.Embedded;

            // Путь до файла с базой данных
            csb.Database = @"D:\users\baa\DB\mac201301010\BUD152.FDB";

            // Настройка параметров "общения" клиента с сервером
            csb.Charset = "WIN1251";
            csb.Dialect = 3;

            // Путь до библиотеки-сервера Firebird
            // Если библиотека находится в тойже папке
            // что и exe фаил - указывать путь не надо
            csb.ClientLibrary = @"C:\Program Files\Firebird\Firebird_2_5\bin\gds32.dll";

            // Настройки аутентификации
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";

            return csb.ToString();
            
        }
    }
}
