using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Tests.Models
{
    class Function
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<SubFunction> SubFunctions { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
