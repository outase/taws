using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tawsCommons.mvc
{
    public interface ITestCaseDatailModelBase
    {
        int id { get; set; }
        string test_case_no { get; set; }
        string execute_order { get; set; }
        string elem_no { get; set; }
        string elem_name { get; set; }
        string sendkey { get; set; }
        string sleep_time { get; set; }
        DateTime create_at { get; set; }
        DateTime update_at { get; set; }
    }
}
