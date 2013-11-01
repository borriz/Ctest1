using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTest1
{
    class esn_class
    {
       public int id;
       public int sotr_id;
    }

    class esn_list
    {
        public List<esn_class> items;

        public esn_list()
        {
            //Конструктор
            items  = new List<esn_class>();
            items.Clear();

        }

        public void get_esn()
        {
            //
        }
    
    
      
    }

}
