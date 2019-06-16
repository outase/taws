using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tawsCommons.mvc;

namespace taws.Models
{
    public class TestCase : ITestCaseModelBase
    {
        public int id { get; set; }
        public int delete { get; set; }
        public string test_case_no { get; set; }
        public string name { get; set; }
        public string test_url { get; set; }
        public string description { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
