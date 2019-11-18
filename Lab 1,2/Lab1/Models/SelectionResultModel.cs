using System.Collections.Generic;
using Lab1.DataAccess.Models;

namespace Lab1.Models
{
    public class SelectionResultModel
    {
        public Phone SelectedPhone { get; set; }

        public List<Phone> ParetoOptimalPhones { get; set; }
    }
}
