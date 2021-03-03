using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Tests.Models
{
    class SubFunction
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public long? FunctionId { get; set; }

        public Function Function { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
