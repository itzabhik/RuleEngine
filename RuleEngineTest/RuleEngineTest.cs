using RulesEngine.RuleEngineContext;
using RulesEngine.RuleModel;
using RulesEngine.Ruleset;
using System;
using System.Collections.Generic;


namespace RuleEngineTest
{
    

    public class Employee: RulesEngine.RuleModel.RuleAwareEntity
    {

        public Employee()
            :base("Employee")
        {
        }

        public long EmployeeNumber { get; set; }

        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlySalary { get; set; }
        public double DefaultBonus { get; set; }

    }

    public class Employee1 : RulesEngine.RuleModel.RuleAwareEntity
    {
        public Employee1()
            : base("Employee1")
        {
        }
        public long EmployeeNumber { get; set; }

        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlySalary { get; set; }

        

        public DateTime BonusPayOutDate { get; set; }
    }
    class RuleEngineTest
    {
        public static string ruleEngineId = Guid.NewGuid().ToString();
        public static void RuleEngineTest1()
        {
           
            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "Employee");

            RuleAwareEntity.CreateDynamicPropertyForType<Employee,double>("Employee", "Bonus");
            RuleAwareEntity.CreateDynamicPropertyForType<Employee,string>("Employee", "BonusCategory");
            List<Employee> employees = new List<Employee>();

            Employee empl = null;

            empl = new Employee();
            empl.EmployeeNumber = 253055;
            empl.FirstName = "Joseph";
            empl.LastName = "Denison";
            empl.HourlySalary = 12.85;
            empl.Age = 40;
            //empl.CreateDynamicProperty<double>("Bonus");
            //empl.CreateDynamicProperty<string>("BonusCategory");
            employees.Add(empl);

            Employee empl1 = new Employee();
            empl1.EmployeeNumber = 204085;
            empl1.FirstName = "Raymond";
            empl1.LastName = "Ramirez";
            empl1.HourlySalary = 9.95;
            empl1.Age = 36;
            empl1.DefaultBonus = 5000;
            //empl1.CreateDynamicProperty<double>("Bonus");
            //empl1.CreateDynamicProperty<string>("BonusCategory");
            employees.Add(empl1);

            Employee empl2 = new Employee();
            empl2.EmployeeNumber = 970044;
            empl2.FirstName = "Christian";
            empl2.LastName = "Riley";
            empl2.HourlySalary = 14.25;
            empl2.Age = 30;
            //empl2.CreateDynamicProperty<double>("Bonus");
            //empl2.CreateDynamicProperty<string>("BonusCategory");
            employees.Add(empl2);

            var ruleset = RulesetBuilder<Employee>
                .Create(conetxt)
                .WithName("Employee Bounus PropertyRule")
                .WithEntryCriteria("EmployeeNumber > 500")
                    .WithRule("HourlySalary>13 And Age>25")
                    .SetPropertyExpression("Bonus", "100.00")
                     .SetPropertyExpression("Bonus", "DoubleProp(\"Bonus\") + 100.00")
                    .SetProperty("BonusCategory", "5 Star")
                    .SetPropertyExpression("Age", "int(Prop(\"Age\"))*2")

                    .Attach()
                .WithRule("HourlySalary>10 And Age>25")
                .SetProperty("Bonus", 80.00)
                .SetProperty("BonusCategory", "4 Star")
                .SetPropertyExpression("Age", "int(Prop(\"Age\"))*4")
                .Attach()
                .WithRule("HourlySalary>8 And Age>25")
                .SetProperty("Bonus", 60.00)
                .SetProperty("BonusCategory", "3 Star")
                .SetPropertyExpression("Age", "int(Prop(\"Age\"))*3")
                .SetPropertyExpression("Bonus", "DoubleProp(\"DefaultBonus\")/2")
                .Attach()
                .Compile();

            foreach (var item in employees)
            {
                ruleset.Execute(item);
            }

           if(ruleset.HasSuccessRule())
            {
                foreach (var item in employees)
                {
                    Console.WriteLine("Bonus of Employee: "
                        + item.FirstName
                        + " is "
                        + item.DoubleProp("Bonus")
                        + " and his Bonus Category is: "
                        + item.StringProp("BonusCategory")
                        + " of age: "
                        +item.Age);

                }
            }

           
        }

        public static void RuleEngineTest2()
        {
            string ruleEngineId = Guid.NewGuid().ToString();
            List<Employee> employees = new List<Employee>();

            Employee empl = null;

            empl = new Employee();
            empl.EmployeeNumber = 253055;
            empl.FirstName = "Joseph";
            empl.LastName = "Denison";
            empl.HourlySalary = 12.85;
            empl.Age = 40;
            empl.DefaultBonus = 1000;
            employees.Add(empl);

            Employee empl1 = new Employee();
            empl1.EmployeeNumber = 204085;
            empl1.FirstName = "Raymond";
            empl1.LastName = "Ramirez";
            empl1.HourlySalary = 9.95;
            empl1.Age = 36;
            empl1.DefaultBonus = 2000;
            employees.Add(empl1);

            Employee empl2 = new Employee();
            empl2.EmployeeNumber = 970044;
            empl2.FirstName = "Christian";
            empl2.LastName = "Riley";
            empl2.HourlySalary = 14.25;
            empl2.Age = 30;
            empl2.DefaultBonus = 3000;
            employees.Add(empl2);

            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "Employee");

            var ruleset = RulesetBuilder<Employee>
                .Create(conetxt)
                .WithName("Employee Bounus PropertyRule")
                .WithEntryCriteria("LongProp(\"EmployeeNumber\") > 500")
              
                .WithRule("HourlySalary>13 And Age>100")
                .SetProperty("Bonus", 100.00)
                .SetProperty("BonusCategory", "5 Star")
                .Attach()
                .WithRule("HourlySalary>10 Or Age>100")
                .SetProperty("Bonus", 80.00)
                .SetProperty("BonusCategory", "4 Star")
                .Attach()
                .WithRule("HourlySalary>8 And Age>100")
                .SetProperty("Bonus", 60.00)
                .SetProperty("BonusCategory", "3 Star")
                .Attach()
                .WithDefaultRule()
                .SetPropertyExpression("Bonus", "DoubleProp(\"DefaultBonus\")/2")
                .SetPropertyExpression("Bonus", "DoubleProp(\"Bonus\")* IntProp(\"Age\")/2")
                .SetProperty("BonusCategory", "1 Star")
                .Compile();



            foreach (var item in employees)
            {
                ruleset.Execute(item);
            }

            if (ruleset.HasSuccessRule())
            {
                foreach (var item in employees)
                {
                    Console.WriteLine("Bonus of Employee: "
                        + item.FirstName
                        + " is "
                        + item.DoubleProp("Bonus")
                        + " and his Bonus Category is: "
                        + item.StringProp("BonusCategory"));

                }
            }


        }

        public static void RuleEngineTest3()
        {
            string ruleEngineId = Guid.NewGuid().ToString();
            List<Employee> employees = new List<Employee>();

            Employee empl = null;

            empl = new Employee();
            empl.EmployeeNumber = 253055;
            empl.FirstName = "Joseph";
            empl.LastName = "Denison";
            empl.HourlySalary = 12.85;
            empl.Age = 40;
            employees.Add(empl);

            Employee empl1 = new Employee();
            empl1.EmployeeNumber = 204085;
            empl1.FirstName = "Raymond";
            empl1.LastName = "Ramirez";
            empl1.HourlySalary = 9.95;
            empl1.Age = 36;
            employees.Add(empl1);

            Employee empl2 = new Employee();
            empl2.EmployeeNumber = 970044;
            empl2.FirstName = "Christian";
            empl2.LastName = "Riley";
            empl2.HourlySalary = 14.25;
            empl2.Age = 30;
            employees.Add(empl2);

            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "Employee");

            var ruleset = RulesetBuilder<Employee>
                .Create(conetxt)
                .WithName("Employee Bounus Property SetRule")
                .WithEntryCriteria("EmployeeNumber < 500")
                .WithRule("HourlySalary>13 And Age>100")
                .SetProperty("Bonus", 100.00)
                .SetProperty("BonusCategory", "5 Star")
                .Attach()
                .WithRule("HourlySalary>10 And Age>25")
                .SetProperty("Bonus", 80.00)
                .SetProperty("BonusCategory", "4 Star")
                .Attach()
                .WithRule("HourlySalary>8 And Age>100")
                .SetProperty("Bonus", 60.00)
                .SetProperty("BonusCategory", "3 Star")
                .Attach()
                .WithDefaultRule()
                .SetProperty("Bonus", 30)
                .SetProperty("BonusCategory", "1 Star")
                .Compile();



            foreach (var item in employees)
            {
                ruleset.Execute(item);
            }

            foreach (var item in employees)
            {
                Console.WriteLine("Bonus of Employee: "
                    + item.FirstName
                    + " is "
                    + item.DoubleProp("Bonus")
                    + " and his Bonus Category is: "
                    + item.StringProp("BonusCategory"));

            }


        }

        public static void RuleEngineTest4()
        {
            string ruleEngineId = Guid.NewGuid().ToString();
            List<Employee> employees = new List<Employee>();

            Employee empl = null;

            empl = new Employee();
            empl.EmployeeNumber = 253055;
            empl.FirstName = "Joseph";
            empl.LastName = "Denison";
            empl.HourlySalary = 12.85;
            empl.Age = 40;
            employees.Add(empl);

            Employee empl1 = new Employee();
            empl1.EmployeeNumber = 204085;
            empl1.FirstName = "Raymond";
            empl1.LastName = "Ramirez";
            empl1.HourlySalary = 9.95;
            empl1.Age = 36;
            employees.Add(empl1);

            Employee empl2 = new Employee();
            empl2.EmployeeNumber = 970044;
            empl2.FirstName = "Christian";
            empl2.LastName = "Riley";
            empl2.HourlySalary = 14.25;
            empl2.Age = 30;
            employees.Add(empl2);

            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "Employee");

            var ruleset = RulesetBuilder<Employee>
                .Create(conetxt)
                .WithName("Employee Bounus Property SetRule")
                .WithEntryCriteria("EmployeeNumber > 500")
                .WithRule("HourlySalary>13 And Age>100")
                .SetProperty("Bonus", 100.00)
                .SetProperty("BonusCategory", "5 Star")
                .Attach()
                .WithRule("HourlySalary>10 And Age>25")
                .SetProperty("Bonus", 80.00)
                .SetProperty("BonusCategory", "4 Star")
                .Attach()
                .WithRule("HourlySalary>8 And Age>100")
                .SetProperty("Bonus", 60.00)
                .SetProperty("BonusCategory", "3 Star")
                .Attach()
                .WithDefaultRule()
                .SetProperty("Bonus", 30)
                .SetProperty("BonusCategory", "1 Star")
                .Compile();

            var ruleset1 = RulesetBuilder<Employee>
                .Create(conetxt)
                .WithName("Employee Bonus Payout Date")
                .WithEntryCriteria("EmployeeNumber > 500")
                .WithRule("GetDoubleProperty(\"Bonus\")=100.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 10))
                .Attach()
                .WithRule("GetDoubleProperty(\"Bonus\")=60.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 9))
                .Attach()
                .WithRule("GetDoubleProperty(\"Bonus\")=80.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 8))
                .Attach()
                .WithDefaultRule()
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 6))
                .Compile();



            foreach (var item in employees)
            {
                ruleset.Execute(item);
                ruleset1.Execute(item);
            }

            foreach (var item in employees)
            {
                Console.WriteLine("Bonus of Employee: "
                    + item.FirstName
                    + " is "
                    + item.DoubleProp("Bonus")
                    + " and his Bonus Category is: "
                    + item.StringProp("BonusCategory")
                    + " payoutdate is :" + item.DateTimeProp("BonusPayOutDate")); ;

            }


        }

        public static void RuleEngineTest5()
        {
            string ruleEngineId = Guid.NewGuid().ToString();
            List<Employee1> employees = new List<Employee1>();

            Employee1 empl = null;

            empl = new Employee1();
            empl.EmployeeNumber = 253055;
            empl.FirstName = "Joseph";
            empl.LastName = "Denison";
            empl.HourlySalary = 12.85;
            empl.Age = 40;
            employees.Add(empl);

            Employee1 empl1 = new Employee1();
            empl1.EmployeeNumber = 204085;
            empl1.FirstName = "Raymond";
            empl1.LastName = "Ramirez";
            empl1.HourlySalary = 9.95;
            empl1.Age = 36;
            employees.Add(empl1);

            Employee1 empl2 = new Employee1();
            empl2.EmployeeNumber = 970044;
            empl2.FirstName = "Christian";
            empl2.LastName = "Riley";
            empl2.HourlySalary = 14.25;
            empl2.Age = 30;
            employees.Add(empl2);

            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "Employee1");

            var ruleset = RulesetBuilder<Employee1>
                .Create(conetxt)
                .WithName("Employee Bounus Property SetRule")
                .WithEntryCriteria("EmployeeNumber > 500")
                .WithRule("HourlySalary>13 And Age>100")
                .SetProperty("Bonus", 100.00)
                .SetProperty("BonusCategory", "5 Star")
                .Attach()
                .WithRule("HourlySalary>10 And Age>25")
                .SetProperty("Bonus", 80.00)
                .SetProperty("BonusCategory", "4 Star")
                .Attach()
                .WithRule("HourlySalary>8 And Age>100")
                .SetProperty("Bonus", 60.00)
                .SetProperty("BonusCategory", "3 Star")
                .Attach()
                .WithDefaultRule()
                .SetProperty("Bonus", 30)
                .SetProperty("BonusCategory", "1 Star")
                .Compile();

            var ruleset1 = RulesetBuilder<Employee1>
                .Create(conetxt)
                .WithName("Employee Bonus Payout Date")
                .WithEntryCriteria("EmployeeNumber > 500")
                .WithRule("GetDoubleProperty(\"Bonus\")=100.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 10))
                .Attach()
                .WithRule("GetDoubleProperty(\"Bonus\")=60.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 9))
                .Attach()
                .WithRule("GetDoubleProperty(\"Bonus\")=80.00 And Age>25")
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 8))
                .Attach()
                .WithDefaultRule()
                .SetProperty("BonusPayOutDate", new DateTime(2021, 1, 6))
                .Compile();



            foreach (var item in employees)
            {
                ruleset.Execute(item);
                ruleset1.Execute(item);
            }

            foreach (var item in employees)
            {
                Console.WriteLine("Bonus of Employee: "
                    + item.FirstName
                    + " is "
                    + item.DoubleProp("Bonus")
                    + " and his Bonus Category is: "
                    + item.StringProp("BonusCategory")
                    + " payoutdate is :" + item.DateTimeProp("BonusPayOutDate")); ;
                Console.Write(":::" + item.BonusPayOutDate);

            }


        }
    }
}
