using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public interface IRuleEngineInstance<TEntity> where TEntity : RuleAwareEntity
    {
        void Execute(IEnumerable<TEntity> data);

        void ExecuteAsync(IEnumerable<TEntity> data);
    }
}
