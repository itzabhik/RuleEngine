using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Tests.Models
{
    class Company
    {
        public string Name { get; set; }

        public long? MainCompanyId { get; set; }

        //public MainCompany MainCompany { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
