using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Tests.Models
{
    class Employee
    {
        public int EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime HireDate { get; set; }

        public long? CompanyId { get; set; }

        public long? CountryId { get; set; }

        public long? FunctionId { get; set; }

        public long? SubFunctionId { get; set; }

        public Company Company { get; set; }

        public Country Country { get; set; }

        public Function Function { get; set; }

        public SubFunction SubFunction { get; set; }

        public int? Assigned { get; set; }
    }
}
