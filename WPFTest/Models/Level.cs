using System;
using System.Collections.Generic;
using System.Text;

namespace WPFTest.Models
{
    public class Level
    {
        public string Name { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public List<int> On { get; set; }
    }
}
