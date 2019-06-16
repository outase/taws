using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tawsCommons.mvc;

namespace taws.Models
{
    public class TestCaseDetail : ITestCaseDatailModelBase
    {
        public int id { get; set; }
        public string test_case_no { get; set; }
        public string execute_order { get; set; }
        public string elem_no { get; set; }
        public string elem_name { get; set; }
        public string sendkey { get; set; }
        public string sleep_time { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
