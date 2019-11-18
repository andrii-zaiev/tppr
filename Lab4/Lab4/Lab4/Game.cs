using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4
{
    public class Game
    {
        public string Name { get; set; }

        public List<string> Rows { get; set; }

        public List<string> Columns { get; set; }

        public bool IsBimatrix { get; set; }

        public List<List<List<double>>> Matrix { get; set; }
    }
}
