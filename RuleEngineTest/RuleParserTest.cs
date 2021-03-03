using RulesEngine.GroupRuleSet;
using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;
using RulesEngine.Ruleset;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace RuleEngineTest
{
    static class RuleParserTest
    {
        public static string ruleEngineId = Guid.NewGuid().ToString();
        public class UserProfile : RuleAwareEntity
        {
            public UserProfile()
            : base("UserProfile")
            {
            }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public long Overtime { get; set; }

            public UserProfileDetails UserProfileDetails { get; set; }
        }

        public class UserProfileDetails : RulesEngine.RuleModel.RuleAwareEntity
        {
            public UserProfileDetails()
                :base("UserProfileDetails")
            {
                LstString = new List<string>();
            }
            public string Department { get; set; }

            public double Salary { get; set; }

            public int LeaveBalance { get; set; }
            public List<string> LstString { get; set; }
        }

        public static void RuleEngineTest1()
        {
            var conetxt = RuleEngineContextHolder.GenerateContext(ruleEngineId, "UserProfile");

            conetxt.AssociateDynamicType<UserProfile>("UserProfile", "y");
            conetxt.AssociateDynamicType<UserProfileDetails>("UserProfileDetails", "x");
        
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfile, double>("UserProfile", "Bonus");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfile, DateTime>("UserProfile", "BonusPayOutDate");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfile, string>("UserProfile", "BonusStatus");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfile, string>("UserProfile", "AgeStatus");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfile, string>("UserProfile", "SalaryStatus");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfileDetails, int>("UserProfileDetails", "Dynamic");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfileDetails, string>("UserProfileDetails", "DynamicStatus");
            RuleAwareEntity.CreateDynamicPropertyForType<UserProfileDetails, List<string>>("UserProfileDetails", "LstStringDynamic",
                new List<string> { "Pamli","Anaya","Siya"});
            //DynamicRuleEntity.CreateDynamicPropertyForType<, string>("UserProfile", "SalaryStatus");
            conetxt
            .AssociateChildDynamicType<UserProfile, UserProfileDetails>("UserProfile", "UserProfileDetails", "UserProfileDetails");

           

            List<UserProfile> lst = GenerateData();

            foreach (var item in lst)
            {
                Console.WriteLine("Employee: "
                    + item.FirstName
                    + " Bonus: "
                    + item.DoubleProp("Bonus")
                    + " Age: "
                    + item.Age
                    + " Salary:"
                    + item.UserProfileDetails.Salary
                    + " Overtime:" + item.Overtime);

                //item.Age = 40;
            }

            Console.WriteLine();

            string s;
          
            
            var mathruleset = RulesetBuilder<UserProfile>
                   .Create(conetxt)
                   .WithName("Maths department Bounus")
                    .WithEntryCriteria("{UserProfileDetails.Department}.ToLower().Contains(\"math\")")
                   // .WithEntryCriteria("{UserProfileDetails.Department}.ToLower().Contains({UserProfileDetails.Department})")
                   /* .WithWhere("{UserProfileDetails.Department}.StartsWith(\"M\") " +
                    "And {UserProfileDetails.Department}.Contains(\"at\")" +
                    "And {UserProfileDetails.Department}.EndsWith(\"h\")")*/
                       .WithRule("({Age}>40 And {Overtime}>40) Or ({UserProfileDetails.Salary}*100< 150)")
                       .SetProperty("Bonus", 100.00)
                       .SetPropertyExpression("UserProfileDetails.LeaveBalance", "2*{UserProfileDetails.LeaveBalance}")

                       .SetProperty("Overtime", 100)
                       .Attach()
                       .WithRule("({Age}<40 And {Overtime}<40) Or ({UserProfileDetails}.{Salary}< 150)")
                       .SetProperty("Bonus", 150.00)
                       .SetProperty("UserProfileDetails.LeaveBalance", 60)
                       .SetProperty<int>("UserProfileDetails.Dynamic", 60)
                       .SetPropertyExpression("Overtime", "{Overtime}*2+1000")
                       .SetPropertyExpression("UserProfileDetails.Dynamic", "{UserProfileDetails.Dynamic}*2+1000")
                       .Attach()
                   .Compile();

            var itruleset = RulesetBuilder<UserProfile>
                  .Create(conetxt)
                  .WithName("IT department Bounus")
                  .WithJobExecutionRule(RulesEngine.RuleExecutionRule.RuleExecutionRuleEnum.FirstMatch)
                  .WithEntryCriteria("{UserProfileDetails}.{Department} = \"IT\"")
                       .WithRule("({Age}<=35 And {Overtime}>10) Or ({UserProfileDetails}.{Salary}*100< 150)")
                       .SetProperty("Bonus", 75.00)
                       .SetProperty("UserProfileDetails.LeaveBalance", 70)
                       .SetProperty("Overtime", 500)
                       .Attach()
                       .WithRule("({Age}>=35 And {Overtime}>40) Or ({UserProfileDetails}.{Salary}> 25)")
                       
                       .SetProperty("Bonus", 85.00)
                       .SetProperty("UserProfileDetails.LeaveBalance", 84)
                       .SetProperty("Overtime", 200)
                       .Attach()
                    .Compile();

            var healthruleset = RulesetBuilder<UserProfile>
                 .Create(conetxt)
                 .WithName("Health department Bounus")
               .WithEntryCriteria("{UserProfileDetails}.{Department} = \"Health\"")
                   .WithRule("({Age}<=35 And {Overtime}>=10) Or ({UserProfileDetails}.{Salary}< 150)")
                   .SetProperty("Bonus", 175.00)
                   .SetProperty("UserProfileDetails.LeaveBalance", 33)
                   .SetProperty("Overtime", 200)
                   .Attach()
                   .WithRule("({Age}>35 And {Overtime}>40) Or ({UserProfileDetails}.{Salary}> 150)")
                   .SetProperty("Bonus", 124.00)
                   .SetProperty("UserProfileDetails.LeaveBalance", 66)
                   .SetProperty("Overtime", 200)
                   .Attach()
               .Compile();

           var dynamicChild = RulesetBuilder<UserProfile>
                 .Create(conetxt)
                 .WithName("Dynamic Status")
              
                   .WithRule("{UserProfileDetails.Dynamic} > 0")
                   .SetPropertyExpression
                   ("UserProfileDetails.DynamicStatus", "string.Concat(\"Dynamic is Greater than 0 for-\", {FirstName}) + {LastName}")
                   
                   .Attach()
                   .WithRule("{UserProfileDetails.Dynamic} < 0")
                   .SetPropertyExpression("UserProfileDetails.DynamicStatus", "\"Dynamic is less than 0 \"")
                 
                   .Attach()
                   .WithDefaultRule()
                   .SetProperty("UserProfileDetails.DynamicStatus", "Dynamic is equal to 0")
               .Compile();

            //lst[0].UserProfileDetails.LstString.Where(s1 => s1.Equals(lst[0].FirstName));
          /*  var a = lst[0];
           var vv= a.UserProfileDetails.EnumerableStringProp("LstStringDynamic").Where(x => !x.Equals(a.FirstName) && !x.Equals("Anaya")).ToList();
            var vvv = a.UserProfileDetails.LstString.Where(x => !x.Equals(a.FirstName)).ToList();
            var vDyna = a.UserProfileDetails.LstString.AsQueryable().Where("!Equals(FirstName)").ToList();*/
           // a.SetPropertyValue("UserProfileDetails.LstString", a.UserProfileDetails.LstString.Where(x => !x.Equals(a.FirstName) && !x.Equals("Anaya")).ToList());

              var dynamicChild1 = RulesetBuilder<UserProfile>
                .Create(conetxt)
                .WithName("Dynamic Status")

                  //.WithRule("{UserProfileDetails.Dynamic} > 0 And {UserProfileDetails.LstString}.Any(Equals(\"Abhik\")) ")
                  //.WithRule("{UserProfileDetails.Dynamic} > 0 And {UserProfileDetails.LstString}.Contains(\"Abhik\") ")
                  //.WithRule("{UserProfileDetails.Dynamic} > 0 And {UserProfileDetails.LstStringDynamic}.Contains(\"Abhik\") ")
                  .WithRule("{UserProfileDetails.Dynamic} > 0 And {UserProfileDetails.LstStringDynamic}.Contains(\"Pamli\") ")
                 .SetPropertyExpression
                   //("UserProfileDetails", "new UserProfileDetails(\"Econo\" as  Department, 5000 as Salary, 45 as LeaveBalance, List.OfString(\"Abhik\") as LstString)")
                   // ("UserProfileDetails", "new UserProfileDetails({UserProfileDetails.DynamicStatus} as  Department, 5000 as Salary, 45 as LeaveBalance, List.OfString(\"Abhik\") as LstString)")
                   ("UserProfileDetails", "new UserProfileDetails({UserProfileDetails.DynamicStatus} as  Department, 5000 as Salary, 45 as LeaveBalance, List.OfString(\"Abhik\") as LstString)")
                     //.SetPropertyExpression("UserProfileDetails.Dynamic", "500")
                     //.SetPropertyExpression("UserProfileDetails.LstString", "{UserProfileDetails.LstStringDynamic}")
                     .SetPropertyExpression("UserProfileDetails.LstString", "List.OfString(\"Abhik\")")
                      //.SetPropertyExpression("UserProfileDetails.LstString", "{a.UserProfileDetails.LstString}.Where(x=>!x.Equals({a.FirstName}) And !x.Equals(\"Anaya\")).ToList()")
                       //.SetPropertyExpression("UserProfileDetails.LstString", "{UserProfileDetails.LstString}.Where(!Equals({a.FirstName}) And !Equals(\"Anaya\")).ToList()")
                       .SetPropertyExpression("UserProfileDetails.LstString", "List.Append({UserProfileDetails.LstString},\"Samik\", \"Saji\")")
                       //.SetPropertyExpression("UserProfileDetails.LstString", "{UserProfileDetails.LstStringDynamic}.Append(\"Samik\", \"Saji\")")
                  //.SetPropertyExpression("UserProfileDetails.LstString", " {UserProfileDetails.LstString}.AddCheck(\"Abhik\")")

                  .Attach()
                  /*.WithRule("{UserProfileDetails.Dynamic} < 0")
                  .SetPropertyExpression<string>("UserProfileDetails.DynamicStatus", "\"Dynamic is less than 0 \"")

                  .Attach()
                  .WithDefaultRule()
                  .SetProperty("UserProfileDetails.DynamicStatus", "Dynamic is equal to 0")*/
              .Compile();



            foreach (var item in lst)
            {
               
                mathruleset.Execute(item);
                itruleset.Execute(item);
                healthruleset.Execute(item);
                 dynamicChild.Execute(item);
               // dynamicChild1.Execute(item);
            }

          foreach (var item in lst)
            {
                Console.WriteLine("Bonus of Employee: "
                    + item.FirstName
                    + " is "
                    + item.DoubleProp("Bonus")
                    + " of age: "
                    + item.Age
                    + " leaveBalance: "
                    + item.UserProfileDetails.LeaveBalance);

                    Console.WriteLine("Overtime of Employee: "
                    + item.FirstName
                    + " Has overtime "
                    + item.Overtime);

                Console.WriteLine("##########################################################################################");
                Console.WriteLine(item.UserProfileDetails.IntProp("Dynamic"));
                Console.WriteLine(item.UserProfileDetails.StringProp("DynamicStatus"));
                Console.WriteLine(item.UserProfileDetails.EnumerableStringProp("LstStringDynamic").Contains("Pamli"));
                Console.WriteLine("##########################################################################################");

            }
            var bonusRuleSet= RulesetBuilder<UserProfile>
               .Create(conetxt)
               .WithName("Bonus Rule")
               .WithPlaceHolder("Tomorrow", "DateTime.Now.AddDays(5)")
               .WithPlaceHolder("DayAfter", "DateTime.Now.AddDays(2)")
               .WithPlaceHolder("Bonus5075", "{Bonus} >50 and {Bonus}<=75")
                 .WithRule("{Bonus} < 50")
                 .SetPropertyExpression("BonusPayOutDate", "{Tomorrow}")
                 .SetPropertyExpression("BonusPayOutDate", "{BonusPayOutDate}.AddDays(1)")
                 .SetPropertyExpression("BonusPayOutDate", "DateTime.Parse(\"12 / 1 / 2012\")")
                 .SetPropertyExpression("BonusPayOutDate", "\"2012-12-02\"")
                 .SetPropertyExpression("BonusPayOutDate", "DateTime(2007, 1, 1)")
                 .Attach()
                 .WithRule("{Bonus5075}")
                 .SetPropertyExpression("BonusPayOutDate", "{DayAfter}")
                 .Attach()
                 .WithRule("{Bonus} >75 and {Bonus}<=100")
                 .SetProperty("BonusPayOutDate", DateTime.Now.AddDays(3))
                 .Attach()
                 .WithDefaultRule()
                 .SetPropertyExpression("BonusPayOutDate", "DateTime.Now.AddDays(1)")
                 .SetPropertyExpression("BonusPayOutDate", "{BonusPayOutDate}.AddDays(1)")
                 .SetPropertyExpression("BonusPayOutDate", "DateTime.Parse(\"12 / 1 / 2012\")")
                 .SetPropertyExpression("BonusPayOutDate", "\"2012-12-02\"")

             .Compile();



           foreach (var item in lst)
           {
               bonusRuleSet.Execute(item);

               Console.WriteLine("Bonus of Employee: "
                   + item.FirstName
                   + " is "
                   + item.DoubleProp("Bonus")
                   + " and the payout date: "
                   + item.DateTimeProp("BonusPayOutDate"));
           }

             TestGrouping(lst, conetxt);
           //TestDiagnostic(lst);
        }



        private static void TestGrouping(List<UserProfile> lst, IRuleEngineContext context)
        {
            Console.WriteLine("##########################################################################################");

            foreach (var item in lst)
            {
              

                Console.WriteLine("Employee: "
                    + item.FirstName
                    + " Bonus: "
                    + item.DoubleProp("Bonus")
                    + " Age: "
                    + item.Age
                    + " Salary:"
                    + item.UserProfileDetails.Salary
                    + " Overtime:" +  item.Overtime);

                //item.Age = 40;
            }
            Console.WriteLine("##########################################################################################");
            Console.WriteLine();


            var grp = GroupRuleSetBuilder<UserProfile, UserProfile>
                .Create(context)
                //.WithAggregateInfo<double>("AvgBonus", AggregateFunction.Average, "{Bonus}*{UserProfileDetails.Salary}")
                //.WithAggregateInfo<double>("AvgBonus", AggregateFunction.Average, "{Bonus}*{Age}")
                //.WithAggregateInfo<double>("AvgBonus", AggregateFunction.Average, "{Bonus}*{UserProfileDetails.Dynamic}")
                .WithAggregateInfo<double>("AvgBonus", AggregateFunction.Average, "{Bonus}")
                .WithAggregateInfo<int>("MaxAge", AggregateFunction.Max, "{Age}")
               //.WithAggregateInfo<int>("MaxAge", AggregateFunction.Max, "int({Age}*{AvgBonus})")
                .WithAggregateInfo<double>("MinSalary", AggregateFunction.Min, "{UserProfileDetails.Salary}")
                .WithAggregateInfo<int>("CountOfRecords", AggregateFunction.Count, "")
               //.WithWhere("{Overtime} >100")
                .WithPlaceHolder("SalaryCheck", "{AvgBonus} + {MaxSalary}")

                //.WithGroupingKey("{Age} + 1, {UserProfileDetails.Salary}")
               .WithGroupingKey("{UserProfileDetails.Dynamic}")
                .WithRuleSet("Bonus Check")
                 .WithHaving("{MaxAge}<40")
                    .WithRule("{Bonus} > {AvgBonus}")
                        .SetProperty("BonusStatus", "Bonus is greater than Average Bonus")
                        .SetAction(t=> Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                    .WithRule("{Bonus} < {AvgBonus}")
                        .SetProperty("BonusStatus", "Bonus is less than Average Bonus")
                         .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                    .DefaultRule()
                        .SetProperty("BonusStatus", "Bonus is equal to Average Bonus")
                         .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                .WithRuleSet("Age Check")
                    .WithRule("{Age} < {MaxAge}")
                        .SetProperty("AgeStatus", "Age is less than Max Age")
                        
                         .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                    .DefaultRule()
                        .SetProperty("AgeStatus", "Age is equal to Max Age")
                         .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                .WithRuleSet("Salary Check")
                    .WithRule("{UserProfileDetails.Salary} > {MinSalary}")
                        .SetProperty("SalaryStatus", "Salary is Greater than min salary")
                        .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                    .WithRule("{UserProfileDetails.Salary} = {MinSalary}")
                        .SetProperty("SalaryStatus", "Salary is equal min Salary")
                         .SetAction(t => Console.WriteLine("Number of Records: " + t.GetPropertyOfType<int>("CountOfRecords")))
                        .Attach()
                    .Attach()
                .Compile();

            grp.Execute(lst);

            foreach (var item in lst)
            {
                Console.WriteLine("Name:" + item.FirstName);
                Console.WriteLine("Overtime:" + item.Overtime);
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Salary Status: "
                    + item.StringProp("SalaryStatus")
                    + "\r\nAge Status: "
                    + item.StringProp("AgeStatus")
                    + "\r\nBonus Status: "
                    + item.StringProp("BonusStatus")
                     + "\r\nMax Age: "
                    + item.IntProp("MaxAge")
                   );

                Console.WriteLine();


            }


        }

        private static void TestDiagnostic(List<UserProfile> lst)
        {
            Console.WriteLine("#############################---Diagnostic---#############################################");

            foreach (var item in lst)
            {
                foreach (var diag in item.GetDiagnosticInfo())
                {
                    Console.WriteLine(diag);
                }
            }
        }

        private static List<UserProfile> GenerateData()
        {
            List<UserProfile> lst = new List<UserProfile>()
            {
                new UserProfile()
                {
                    FirstName = "Abhik",
                    LastName = "Rakshit",
                    Age = 40,
                    Overtime = 22,

                    UserProfileDetails=new UserProfileDetails()
                    {
                        Department="Math",
                        Salary=100.20,
                        LeaveBalance=23,

                    }
                },
                new UserProfile()
                {
                    FirstName = "Pamli",
                    LastName = "Rakshit",
                    Age = 35,
                    Overtime = 18,

                    UserProfileDetails=new UserProfileDetails()
                    {
                        Department="IT",
                        Salary=75.45,
                        LeaveBalance=20,
                    }
                },

                new UserProfile()
                {
                    FirstName = "Anaya",
                    LastName = "Rakshit",
                    Age = 25,
                    Overtime = 10,

                    UserProfileDetails=new UserProfileDetails()
                    {
                        Department="Health",
                        Salary=234.56,
                        LeaveBalance=27,
                    }
                },
            };

            return lst;
        }
    }

    
}
