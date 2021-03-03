using RulesEngine.RuleModel;
using System;


namespace RulesEngine.Tests.Models.Blogs
{
    public class Post : RuleAwareEntity
    {
        public Post() : base("Post")
        {
        }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public int NumberOfReads { get; set; }

        public DateTime PostDate { get; set; }

        public string Satus { get; set; }
    }


}
