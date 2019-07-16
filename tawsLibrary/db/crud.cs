using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tawsLibrary.db
{
    class crud
    {
        public void apply()
        {
            var dbc = new Connection();

            //Select
            {
                var allData = dbc.Table.ToList();
                var filterData = dbc.Table.Where(w => w.ToString() == "").ToList();
            }
        }
    }
}
