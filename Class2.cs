using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTest1
{
    static class FBDBClass
    {
        public string GetConnFBstring()
        { 
             FbConnectionStringBuilder csb = new FbConnectionStringBuilder();

            // Указываем тип используемого сервера
            csb.ServerType = FbServerType.Embedded;

            // Путь до файла с базой данных
            csb.Database = @"D:\users\baa\DB\v14868mac\BUD107.GDB";

            // Настройка параметров "общения" клиента с сервером
            csb.Charset = "WIN1251";
            csb.Dialect = 3;

            // Путь до бибилиотеки-сервера Firebird
            // Если библиотека находится в тойже папке
            // что и exe фаил - указывать путь не надо
            csb.ClientLibrary = @"C:\Program Files\Firebird\Firebird_2_5\bin\gds32.dll";

            // Настройки аутентификации
            csb.UserID = "SYSDBA";
            csb.Password = "masterkey";

            return csb.Tostring();
            
        };
    }
}
