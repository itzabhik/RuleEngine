using RulesEngine.RuleEngineContext;
using RulesEngine.RuleModel;
using RulesEngine.Ruleset;
using RulesEngine.Tests.Models.Blogs;
using System;
namespace RulesEngine.Tests
{
    public static class  RuleEngineDynamicTest
    {
        public static void DynamicPropertyWithCollection()
        {
            var data = BlogData.PopulateDynamicData(50,100);

            var conetxt = RuleEngineContextHolder.GenerateContext("Blog");
            conetxt.AssociateDynamicType<RuleAwareEntity>("Blog", "y");
            conetxt.AssociateDynamicType<RuleAwareEntity>("Post", "x");
            conetxt
                .AssociateChildDynamicType<RuleAwareEntity, RuleAwareEntity>("Blog", "Post", "Posts");
            conetxt
                .AssociateChildDynamicType<RuleAwareEntity, RuleAwareEntity>("Post", "Blog", "Blog");

            var blogruleSet = RulesetBuilder<RuleAwareEntity>
                 .Create(conetxt)
                 .WithJobExecutionRule(RuleExecutionRule.RuleExecutionRuleEnum.FirstMatch)
                 .WithName("Maths department Bounus")
                 .WithEntryCriteria("{Name}.Contains(\"2\")")
                     //.WithRule("{Posts}.Any({x:Blog.y:Name}.Contains(\"2\")) And {Created} < DateTime.Now.AddDays(2)")
                      //.WithRule("{Posts}.Any({x:Blog.Name}.Contains(\"2\")) And {Created} < DateTime.Now.AddDays(2)")
                    // .WithRule("{Posts}.Any({x:Title}.Contains(\"2\")) And {Created} < DateTime.Now.AddDays(2)")
                     .WithRule("{Posts}.Any({x:Blog.Posts}.Any({x:Title}.Contains(\"2\"))) And {Created} < DateTime.Now.AddDays(2)")
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 1")
                     .SetAction(t => Console.WriteLine("1 Rule"))
                     .Attach()
                      .WithRule("{Posts}.Any({x:Blog.Posts}.Any({x:Title}.Contains(\"2\"))) And {Created} < DateTime.Now.AddDays(2)")
                     //.WithRule("{Posts}.Any({x:Title}.Contains(\"2\")) And {Created} > DateTime.Now.AddDays(2) And {Created} < DateTime.Now.AddDays(10)")
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 5")
                      .SetAction(t => Console.WriteLine("2 Rule"))
                     .Attach()
                   .WithDefaultRule()
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 50")
                     .SetPropertyExpression("Posts.First().NumberOfReads", "{LikeCount} + 50")
                     .SetAction(t => Console.WriteLine("Default Rule"))
                 .Compile();

            foreach (var item in data)
            {

                blogruleSet.Execute(item);
            }

            foreach (var item in data)
            {
                Console.WriteLine("###################");

                Console.WriteLine("Blog Name: " + item.Prop("Name"));
                Console.WriteLine("Created: " + item.Prop("Created"));
                Console.WriteLine("LikeCount: " + item.Prop("LikeCount"));
                Console.WriteLine("Tomorrow: " + DateTime.Now.AddDays(2));
               // Console.WriteLine("Filter : " + (item.Posts.Any(s => s.Title.Contains("2")) && item.Created > DateTime.Now.AddDays(2)));
                Console.WriteLine("###################");

            }


        }

        public static void DynamicPropertyWithCollectionPlaceHolder()
        {
            var data = BlogData.PopulateDynamicData();

            var conetxt = RuleEngineContextHolder.GenerateContext("Blog");
            conetxt.AssociateDynamicType<RuleAwareEntity>("Blog", "y");
            conetxt.AssociateDynamicType<RuleAwareEntity>("Post", "x");
            conetxt
                .AssociateChildDynamicType<RuleAwareEntity, RuleAwareEntity>("Blog", "Post", "Posts");
            conetxt
                .AssociateChildDynamicType<RuleAwareEntity, RuleAwareEntity>("Post", "Blog", "Blog");


            var blogruleSet = RulesetBuilder<RuleAwareEntity>
                 .Create(conetxt)
                 .WithName("Maths department Bounus")
                 .WithPlaceHolder("PostWith2", "{Posts}.Any({x:Blog.Name}.Contains(\"2\"))")
                 .WithEntryCriteria("{Name}.Contains(\"2\")")
                     .WithRule("{PostWith2} And {Created} < DateTime.Now.AddDays(2)")
                     //.WithRule("{Posts}.Any({x:Title}.Contains(\"2\")) And {Created} < DateTime.Now.AddDays(2)")
                     //.WithRule("{Posts}.Any({x:Blog.Posts}.Any({x:Title}.Contains(\"2\"))) And {Created} < DateTime.Now.AddDays(2)")
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 1")
                     .SetAction(t => Console.WriteLine("1 Rule"))
                     .Attach()
                      .WithRule("{PostWith2} And {Created} > DateTime.Now.AddDays(2) And {Created} < DateTime.Now.AddDays(10)")
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 5")
                      .SetAction(t => Console.WriteLine("2 Rule"))
                     .Attach()
                     .WithDefaultRule()
                     .SetPropertyExpression("LikeCount", "{LikeCount} + 50")
                     //.SetPropertyExpression<int>("Posts.First().NumberOfReads", "{LikeCount} + 50")
                     .SetAction(t => Console.WriteLine("Default Rule"))
                 .Compile();

            foreach (var item in data)
            {

                blogruleSet.Execute(item);
            }

            foreach (var item in data)
            {
                Console.WriteLine("###################");

                Console.WriteLine("Blog Name: " + item.Prop("Name"));
                Console.WriteLine("Created: " + item.Prop("Created"));
                Console.WriteLine("LikeCount: " + item.Prop("LikeCount"));
                Console.WriteLine("Tomorrow: " + DateTime.Now.AddDays(2));
                // Console.WriteLine("Filter : " + (item.Posts.Any(s => s.Title.Contains("2")) && item.Created > DateTime.Now.AddDays(2)));
                Console.WriteLine("###################");

            }


        }
    }
}
