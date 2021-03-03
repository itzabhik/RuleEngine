using RulesEngine.RuleModel;

namespace RulesEngine.Rules
{
    interface IPropertyRule<TEntity>: IRule<TEntity> where TEntity : RuleAwareEntity
    {
        void SetPropertyValues(TEntity entity);
    }
}