using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tawsCommons.mvc
{
    public interface ITestCaseModelBase
    {
        int id { get; set; }
        int delete { get; set; }
        string test_case_no { get; set; }
        string name { get; set; }
        string test_url { get; set; }
        string description { get; set; }
        DateTime create_at { get; set; }
        DateTime update_at { get; set; }
    }
}
