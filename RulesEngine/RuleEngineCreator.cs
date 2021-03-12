using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public static class RuleEngineCreator
    {
        public static IRuleEngine<TEntity> CreateRuleEngineFromXml<TEntity>() where TEntity : RuleAwareEntity
        {
            return null;
        }

        public static IRuleEngine<TEntity> CreateRuleEngineFromExcel<TEntity>() where TEntity : RuleAwareEntity
        {
            return null;
        }

        public static IRuleEngine<TEntity> CreateRuleEngineFromJson<TEntity>() where TEntity : RuleAwareEntity
        {
            return null;
        }

        public static RuleEngineBuilder<TEntity> Create<TEntity>() where TEntity : RuleAwareEntity
        {
            return null;
        }
    }
}
