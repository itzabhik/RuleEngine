using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using RuleEngineTest.RuleEntity;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;

namespace RuleEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string absc = "(a & b | C) & (d | f) | (e & f)";
            var splitted = absc.Split(new[] { "&","|" },StringSplitOptions.None);
            double f = 10;
            int? z = 5;

            object val = f * z;

            List<int> gg = new List<int>();
           

            // absc.Split();

            //DynamicExpressionParser.ParseLambda<Customer>()
            //ObjectTest();
            //RuleSetTest();
            //DynamicProperty();
            //DynamicExpressionParser.ParseLambda<Customer,bool>()
            //DynamicProperty1();
            //ObjectTest1();
            //RuleEngineTest.RuleEngineTest1();
            //ObjectTest1();
            //PlaceHolderTester();

           RuleParserTest.RuleEngineTest1();

            //StringBuilder builder = new StringBuilder();
            //Getdefault(typeof(Customer));

            //TestDateTime();

            // RuleEngineInstanceTest.InstanncePropertyWithCollection();
            //RuleEngineDynamicTest.DynamicPropertyWithCollection();
           // RuleEngineDynamicTest.DynamicPropertyWithCollectionPlaceHolder();

            //var val= Getdefault(typeof(int));
            Console.ReadLine();
        }

        public static object Getdefault(Type type)
        {
            Console.WriteLine(type.IsClass);
            if (type.IsValueType)
            {
                if(type==typeof(int))
                {

                }
                return Activator.CreateInstance(type);
            }
            if (type == typeof(string))
                return string.Empty;
            return null;
        }

        private static void PlaceHolderTester()
        {
            string expression = "{Boy} is a good {Boy}";
            PlaceHolderTextParser pp = new PlaceHolderTextParser(expression);
            List<string> lst = new List<string>();
            int sub = 0;
            string placeHolder;
            string placeHolderWithParenthesis;
            while (pp.NextPlaceHolder(out placeHolder, out placeHolderWithParenthesis))
            {
                Console.WriteLine(placeHolder);
                Console.WriteLine(placeHolderWithParenthesis);
                //lst.Add(placeHolderWithParenthesis);
                expression = Replace(expression, pp.startplaceHolderpos - sub, pp.endPlaceHolderpos, placeHolder);

                pp = new PlaceHolderTextParser(expression);
            }

            foreach (var item in lst)
            {

            }
            Console.WriteLine(expression);
        }

        private static string Replace(string expression, int startIndex, int endIndex, string placeHolder)
        {
            return  expression.Remove(startIndex, (endIndex - startIndex)+1)
                .Insert(startIndex, placeHolder);
        }

        public class Claim
        {
            public decimal? Balance { get; set; }
            public List<string> Tags { get; set; }
        }

        private static void TestDateTime()
        {
            var claim1 = new Claim { Balance = 100, Tags = new List<string> { "Blah", "Blah Blah" } };
            var claim2 = new Claim { Balance = 500, Tags = new List<string> { "Dummy Tag", "Dummy tag 1", "New" } };

            var claims = new List<Claim> { claim1, claim2 };

            var whrNonDynamic = claims.Where(c => c.Tags.Any());
            var whrDynamic = claims.AsQueryable().Where("Tags.Any(Contains(\"New\") Or Contains(\"Blah\"))");

            List<Customer> lst = new List<Customer>();

            
           
            Type tt=lst.GetType();

        }
        private static void DynamicProperty1()
        {
            var props = new DynamicProperty[]
            {
                new DynamicProperty("Name", typeof(string)),
                new DynamicProperty("Age", typeof(int))
            };

            Type type = DynamicClassFactory.CreateType(props);

            var dynamicClass = Activator.CreateInstance(type) as DynamicClass;
            dynamicClass.SetDynamicPropertyValue("Name", "Abhik");
            dynamicClass.SetDynamicPropertyValue("Age", 40);

            var dynamicClass1 = Activator.CreateInstance(type) as DynamicClass;
            dynamicClass1.SetDynamicPropertyValue("Name", "Pamli");
            dynamicClass1.SetDynamicPropertyValue("Age", 35);

            List<DynamicClass> customer = new List<DynamicClass>()
            {
                dynamicClass,
                dynamicClass1
            };

            var cust = customer.AsQueryable().Where("GetDynamicPropertyValue(Age)>20");

            foreach (DynamicClass item in cust)
            {
                Console.WriteLine(item.GetDynamicPropertyValue("Name"));
            }

            Console.Read();

        }

        private static void DynamicProperty()
        {

            List<DynamicProperty> properties = new List<DynamicProperty>
            {
                new DynamicProperty("Name",typeof(string)),
                 new DynamicProperty("Age",typeof(int)),
            };

            List<dynamic> customer = new List<dynamic>();

            var obj = new RuleholderDynamic("Customer", properties);
            obj.SetMember("Age", 32);
            obj.SetMember("Name", "Abhik");


            var obj1 = new RuleholderDynamic("Customer", properties);
            obj1.SetMember("Name", "Pamli");
            obj1.SetMember("Age", 23);


            customer.Add(obj);
            customer.Add(obj1);

            var cust = customer.AsQueryable().Where("Age>20");

            foreach (dynamic item in cust)
            {
                Console.WriteLine(item.Name);
            }

            Console.Read();
        }

        private static void RuleSetTest()
        {
            List<Property> properties = new List<Property>
            {
                new Property("Name",typeof(string)),
                new Property("Age",typeof(int)),
            };

            List<dynamic> customer = new List<dynamic>();

            var obj = new RuleHolder("Customer", properties);
            obj.SetMember("Name", "Abhik");
            obj.SetMember("Age", 32);

            var obj1 = new RuleHolder("Customer", properties);
            obj1.SetMember("Name", "Pamli");
            obj1.SetMember("Age", 23);


            customer.Add(obj);
            customer.Add(obj1);

            var cust = customer.AsQueryable().Where("Age>20");

            foreach (dynamic item in cust)
            {
                Console.WriteLine(item.Name);
            }

            Console.Read();

        }

        private static void ObjectTest()
        {
            List<Customer> lst = new List<Customer>
            {
                new Customer()
                {
                    Age=10,
                    Name="Abhik",
                    CompareAge=3
                },
                new Customer()
                {
                    Age=11,
                    Name="Pamli",
                    CompareAge=14
                }
            };


            var cust = lst.AsQueryable().Where("(Age)>CompareAge + 2");
            foreach (dynamic item in cust)
            {
                Console.WriteLine(item.Name);
                item.Price = 100;
            }

            Console.Read();
        }

        private static void ObjectTest1()
        {

            var cust =new Customer()
            {
                Age = 10,
                Name = "Abhik",
                CompareAge = 3,
                OrderDate = DateTime.Now
            };
            Type t = cust.GetType();
            var p = t.GetProperty("Age");
            var tt1 = cust as RuleAwareEntity;
            Type tt = tt1.GetType();
            var pp = tt.GetProperty("Age");

            //DynamicRuleEntity.CreateDynamicPropertyForType<Customer>("Customer", "Bonus", typeof(int));
            List<Customer> lst = new List<Customer>
            {
                new Customer()
                {
                    Age=10,
                    Name="Abhik",
                    CompareAge=3,
                    OrderDate = DateTime.Now
                },
                new Customer()
                {
                    Age=11,
                    Name="Pamli",
                    CompareAge=14,
                    OrderDate = DateTime.Now
                },
                   new Customer()
                {
                    Age=11,
                    Name="Anaya",
                    CompareAge=14,
                    OrderDate = DateTime.Now
                }
            };

            


            //var grps = lst.Sum();
            lst.GroupBy(c => new { c.Age, c.CompareAge }).Select(g=>new { Data = g.ToList(), MaxSum = g.Count() });
            //vapr = "C>10";
            //.Select("new(Key, Count() AS Count)")

            //var str = "new RuleKey(int(Prop(\"Age\")) as SetPropertyValue(\"Age\"))";
            var str = "new ((int(Prop(\"Age\")) + int(Prop(\"CompareAge\"))) as Age1, int(Prop(\"CompareAge\")) + 2 as Age2 )";

          
          

            //var lamdaexpr = DynamicExpressionParser.ParseLambda<Customer, DynamicClass>(new ParsingConfig(), true, str);


            Delegate expre=DynamicExpressionParser.ParseLambda(new ParsingConfig(), true, typeof(Customer), null, str).Compile();


            //var str = StringRulePLaceHolderParser
            //.ParseGroupStringForPlaceHolder<Customer>("Customer", "{({Age} +{CompareAge})} ,{CompareAge},{Bonus}");

            //var str = StringRulePLaceHolderParser
            //.ParseGroupStringForPlaceHolder<Customer>("Customer", "{Age}+{CompareAge},{CompareAge}-2,{Bonus}*2");

            //var exprrr = DynamicExpressionParser
            //.ParseLambda<RulesEngine.RuleModel.DynamicRuleEntity, IGrouping<DynamicClass, RulesEngine.RuleModel.DynamicRuleEntity>>(new ParsingConfig(), true, str);

            //var str = "new RuleKey(Age1 as Age)";

            //var test = lst.GroupBy(c => c.Age+c.CompareAge).AsQueryable();

            var test = lst.GroupBy(t11=> expre.DynamicInvoke(t11));



            /*var test = lst.AsQueryable().GroupBy(new ParsingConfig
            {
                DisableMemberAccessToIndexAccessorFallback = false,
                //CustomTypeProvider = new CustomTypeProvider()

            }, str);*/

         


            /*foreach (var item in test)
            {
                //var val = (IGrouping<DynamicClass, RulesEngine.RuleModel.RuleAwareEntity>)item;
               var val = (IGrouping<object, RulesEngine.RuleModel.RuleAwareEntity>)item;

                //var val = (IGrouping<RulesEngine.RuleModel.DynamicRuleEntity, RulesEngine.RuleModel.DynamicRuleEntity>)item;

                var exprep = DynamicExpressionParser
                    .ParseLambda<RulesEngine.RuleModel.RuleAwareEntity, int>(new ParsingConfig(), true, "IntProp(\"Age\")+2").Compile();

                object sumage = val.Sum(i => exprep(i));

                List<RulesEngine.RuleModel.RuleAwareEntity> data = val.AsQueryable().ToList();


            }*/

            foreach (var item in test)
            {
                //var val = (IGrouping<DynamicClass, RulesEngine.RuleModel.RuleAwareEntity>)item;
                var val = (IGrouping<object, Customer>)item;

                //var val = (IGrouping<RulesEngine.RuleModel.DynamicRuleEntity, RulesEngine.RuleModel.DynamicRuleEntity>)item;

                //var exprep = DynamicExpressionParser
                // .ParseLambda<Customer, int>(new ParsingConfig(), true, "IntProp(\"Age\")+2").Compile();

                var exprep = DynamicExpressionParser
                  .ParseLambda(new ParsingConfig(), true,typeof(Customer),typeof(int), "IntProp(\"Age\")+2").Compile();

                //val.Sum()

               // object sumage = val.Sum(i => exprep.DynamicInvoke(i));

                // List<RulesEngine.RuleModel.RuleAwareEntity> data = val.AsQueryable().ToList();


            }




            Console.Read();
        }
    }

   
}


class Customer : RulesEngine.RuleModel.RuleAwareEntity
{

    public Customer()
            : base( "Customer")
    {
    }
    public int Age { get; set; }
    public String Name { get; set; }

    public DateTime OrderDate { get; set; }
    public int CompareAge { get; set; }

    
}



