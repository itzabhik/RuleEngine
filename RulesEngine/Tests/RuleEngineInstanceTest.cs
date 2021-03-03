using RulesEngine.RuleEngineContext;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Ruleset;
using RulesEngine.Tests.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.Tests
{
    public static class RuleEngineInstanceTest
    {

        public static void InstanncePropertyWithCollection()
        {
            var data = BlogData.PopulateTestData();
            Console.WriteLine("Data Created");

            var conetxt = RuleEngineContextHolder.GenerateContext("Blog");

            var blogruleSet = RulesetBuilder<Blog>
                .Create(conetxt)
                .WithName("Maths department Bounus")
                .WithEntryCriteria("{Name}.Contains(\"2\") Or {Name}.Contains(\"1\")")
                     //.WithRule("{Posts}.Any({x:Title}.Contains(\"2\")) And {Created} > DateTime.Now.AddDays(2)")
                     //.WithRule("{Posts}.Any({x:Blog.y:Posts}.Any({x:Title}.Contains(\"2\"))) And {Created} < DateTime.Now.AddDays(2)")
                    .WithRule("{Posts}.Any({x:Blog.Name}.Contains(\"2\")) And {Created} > DateTime.Now.AddDays(2)")
                    .SetPropertyExpression("LikeCount", "{LikeCount} + 1")
                    .SetAction(t => Console.WriteLine("Instance 1 Rule"))
                    .Attach()
                     .WithRule("{Posts}.Any({x:Title}.Contains(\"2\")) And {Created} > DateTime.Now.AddDays(2) And {Created} < DateTime.Now.AddDays(10)")
                    .SetPropertyExpression("LikeCount", "{LikeCount} + 5")
                     .SetAction(t => Console.WriteLine(" Instance 2 Rule"))
                    .Attach()
                    .WithDefaultRule()
                    .SetPropertyExpression("LikeCount", "{LikeCount} + 50")
                    //.SetPropertyExpression<int>("Posts.First().NumberOfReads", "{LikeCount} + 50")
                    .SetAction(t => Console.WriteLine("InstanceDefault Rule"))
                .Compile();

          

            foreach (var item in data)
            {
                
                blogruleSet.Execute(item);
            }

            foreach (var item in data)
            {
                Console.WriteLine("###################");

                Console.WriteLine("Blog Name: " + item.Name);
                Console.WriteLine("Created: " + item.Created);
                Console.WriteLine("LikeCount: " + item.LikeCount);
                Console.WriteLine("Tomorrow: " + DateTime.Now.AddDays(2));
                Console.WriteLine("Filter : " + (item.Posts.Any(s=>s.Title.Contains("2")) && item.Created > DateTime.Now.AddDays(2)));
                Console.WriteLine("###################");

            }

        }

        public static void TestPlaceholder()
        {
            var data = BlogData.PopulateTestData();
            var conetxt = RuleEngineContextHolder.GenerateContext("Blog");
            var blogruleSet = RulesetBuilder<Blog>
                  .Create(conetxt)
                  .WithName("Maths department Bounus")
                  .WithPlaceHolder("Tomorrow", "DateTime.Now.AddDays(2)")
                  .WithEntryCriteria("{Name}.Contains(\"2\")")
                      .WithRule("{Posts}.Any({Title}.Contains(\"2\")) And {Created} < {Tomorrow}")
                      .SetPropertyExpression("LikeCount", "{LikeCount} + 1")
                      .Attach()
                       .WithRule("{Posts}.Any({Title}.Contains(\"2\")) And {Created} > DateTime.Now.AddDays(2) And {Created} < DateTime.Now.AddDays(10)")
                      .SetPropertyExpression("LikeCount", "{LikeCount} + 5")
                      .Attach()
                      .WithDefaultRule()
                      .SetPropertyExpression("LikeCount", "{LikeCount} + 50")
                      .SetPropertyExpression("Posts.First().NumberOfReads", "{LikeCount} + 50")
                  .Compile();

            
            foreach (var item in data)
            {

                blogruleSet.Execute(item);
            }

            foreach (var item in data)
            {
                Console.WriteLine("###################");

                Console.WriteLine("Blog Name: " + item.Name);
                Console.WriteLine("Created: " + item.Created);
                Console.WriteLine("LikeCount: " + item.LikeCount);
                Console.WriteLine("Tomorrow: " + DateTime.Now.AddDays(2));
                Console.WriteLine("Filter : " + (item.Posts.Any(s => s.Title.Contains("2")) && item.Created > DateTime.Now.AddDays(2)));
                Console.WriteLine("###################");

            }

        }
    }
}
