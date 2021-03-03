using RulesEngine.RuleEngineContext;
using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;


namespace RulesEngine.Tests.Models.Blogs
{
    public class Blog:RuleAwareEntity
    {
        public Blog() : base("Blog")
        {
            Posts = new List<Post>();
        }

        public int BlogId { get; set; }
        public string Name { get; set; }
        public int? NullableInt { get; set; }
        public virtual List<Post> Posts { get; set; }

        public DateTime Created { get; set; }

        public int LikeCount { get; set; }

        public new List<RuleAwareEntity> EnumerableComplexProp(string property)
        {
            return base.EnumerableComplexProp(property);
        }


    }

    static class BlogData
    {
        public static string ruleEngineId = Guid.NewGuid().ToString();

        static readonly Random Rnd = new Random(1);
        public static List<Blog> PopulateTestData(int blogCount = 35, int postCount = 40)
        {
           
           // var conetxt= RuleEngineContextHolder.GenerateContext(ruleEngineId, "Blog1");

            List<Blog> lstBlog = new List<Blog>();

            for (int i = 0; i < blogCount; i++)
            {
                var blog = new Blog() {Id= i + 1, Name = "Blog" + (i + 1), Created = DateTime.Today.AddDays(-Rnd.Next(0, 100)) };

                
                lstBlog.Add(blog);

                for (int j = 0; j < postCount; j++)
                {
                    var post = new Post()
                    {
                        Id= j + 1,
                        Blog = blog,
                        Title = $"Blog {i + 1} - Post {j + 1}",
                        Content = "My Content",
                        PostDate = DateTime.Today.AddDays(-Rnd.Next(0, 100)).AddSeconds(Rnd.Next(0, 30000)),
                        NumberOfReads = Rnd.Next(0, 5000)
                    };

                    blog.Posts.Add(post);
                }
                Console.WriteLine("Item Created: " + i);
            }


            return lstBlog;

        }

        public static List<RuleAwareEntity> PopulateDynamicData(int blogCount = 35, int postCount = 40)
        {
            CreateBlogClass();

            List<RuleAwareEntity> lstBlog = new List<RuleAwareEntity>();

            for (int i = 0; i < blogCount; i++)
            {
                var blog = new RuleAwareEntity("Blog");
                blog.Id = i + 1;
                blog.TrySetDynamicProperty("Name", "Blog" + (i + 1));
                blog.TrySetDynamicProperty("Created" ,DateTime.Today.AddDays(-Rnd.Next(0, 100)));
                lstBlog.Add(blog);
                for (int j = 0; j < postCount; j++)
                {
                    var post = new RuleAwareEntity("Post");
                    post.Id = j + 1;
                    post.TrySetDynamicProperty("Blog", blog);
                    post.TrySetDynamicProperty("Title" ,$"Blog {i + 1} - Post {j + 1}");
                    post.TrySetDynamicProperty("Content" , "My Content");
                    post.TrySetDynamicProperty("PostDate" ,DateTime.Today.AddDays(-Rnd.Next(0, 100)).AddSeconds(Rnd.Next(0, 30000)));
                    post.TrySetDynamicProperty("NumberOfReads", Rnd.Next(0, 5000));

                    blog.EnumerableComplexProp("Posts").Add(post);
                }

                Console.WriteLine("Item Created: " + i);

            }
            return lstBlog;
        }

        private static void CreateBlogClass()
        {
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int>("Blog", "BlogId");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, string>("Blog", "Name");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int?>("Blog", "NullableInt");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, DateTime>("Blog", "Created");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int>("Blog", "LikeCount");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, List<RuleAwareEntity>>("Blog", "Posts");

            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int>("Post", "PostId");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, string>("Post", "Title");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, string>("Post", "Content");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int>("Post", "BlogId");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, RuleAwareEntity>("Post", "Blog");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, int>("Post", "NumberOfReads");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, DateTime>("Post", "PostDate");
            RuleAwareEntity.CreateDynamicPropertyForType<RuleAwareEntity, string>("Post", "Satus");

        }
    }


}
