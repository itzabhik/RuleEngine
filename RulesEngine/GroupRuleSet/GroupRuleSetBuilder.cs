using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Rules;
using RulesEngine.Ruleset;
using System;
using System.Collections.Generic;

namespace RulesEngine.GroupRuleSet
{
    public class GroupRuleSetBuilder<TEntity,TKey> where TEntity : RuleAwareEntity
    {
       
        private string _name;
        private string _description;
        private string _groupKey;
      
        private readonly List<AggregatePropertyMetadata<TEntity>> _aggregateProps=new List<AggregatePropertyMetadata<TEntity>>();
        private readonly RuleEngineContext.RuleEngineContext _context;
        private List<IRuleset<TEntity>> _ruleSets = new List<IRuleset<TEntity>>();
        private List<RulesetBuilder<TEntity>> _ruleSetBuilders = new List<RulesetBuilder<TEntity>>();
        private Func<TEntity, TKey> _keySelector;

        private IRuleExpressionParser<TEntity> _ruleparser;
        internal IPropertyRuleExpressionParser<TEntity> _propertyparser;
       
        private IGroupKeyExpressionParser<TEntity> _groupKeyExpressionParser;

        private Dictionary<string, string> _placeHolders = new Dictionary<string, string>();

        private string _entryCriteria;
        private Func<TEntity, bool> _rulefunc;

        public object RuleEngineContextGenerator { get; private set; }

        private GroupRuleSetBuilder(RuleEngineContext.RuleEngineContext context)
        {
          
            _ruleparser = RuleExpressionParserFactory.CreateStringRuleParser<TEntity>();
            _propertyparser = RuleExpressionParserFactory.CreateStringPropertyRuleParser<TEntity>();
           
            _groupKeyExpressionParser = RuleExpressionParserFactory.CreateGroupKeyParser<TEntity>();
            this._context = context;
        }
        public GroupRuleSetBuilder<TEntity, TKey> WithName(string name)
        {
            name = name ?? Guid.NewGuid().ToString();
            _name = name;
            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> WithDescription(string description)
        {
            _description = description ?? string.Empty;
            return this;
        }

        public GroupRuleSetBuilder<TEntity,TKey> WithPlaceHolder(string placeHolderName, string placeHolderExpression)
        {
            if (_placeHolders.ContainsKey(placeHolderName))
            {
                throw new PlaceHolderException(placeHolderName, _name, _context.RuleEngineId);
            }
            _placeHolders.Add(placeHolderName, string.Format("{0}{1}{2}", "(", placeHolderExpression, ")"));

            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> WithWhere(string entryCriteria)
        {
            _entryCriteria = entryCriteria;
            return this;
        }
        public GroupRuleSetBuilder<TEntity, TKey> WithWhere(Func<TEntity, bool> rulefunc)
        {
            _rulefunc = rulefunc;
            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> WithGroupingKey(string groupKey)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                throw new ArgumentException("Grouping Key can't be null", nameof(groupKey));
            }
            _groupKey = groupKey;

            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> WithGroupingKey(Func<TEntity, TKey> keySelector)
        {
            _keySelector = keySelector ?? throw new ArgumentException("Grouping Key can't be null", nameof(keySelector));

            return this;
        }

        public GroupRuleSetBuilder<TEntity, TKey> WithAggregateInfo<TType>(string propertyName, AggregateFunction function,string agrregateOn)
        {
            AggregatePropertyMetadata<TEntity> aggregatePropertyMetadata=null;
            
            if (function==AggregateFunction.Count )
            {
                var parser = RuleExpressionParserFactory.CreateAggregateFunctionParser<TEntity, bool>();
                aggregatePropertyMetadata = new CountAggregatePropertyMetadataType<TEntity>(_context, propertyName,typeof(TType), agrregateOn, parser);
            }
            else
            {
                var parser = RuleExpressionParserFactory.CreateAggregateFunctionParser<TEntity, TType>();
                aggregatePropertyMetadata = new AggregatePropertyMetadataType<TEntity, TType>
                                                            (_context, propertyName, function, agrregateOn, parser);
            }
           

            _aggregateProps.Add(aggregatePropertyMetadata);
            return this;
        }
        public GroupRuleSetBuilder<TEntity, TKey> WithAggregateInfo(string propertyName,Type type, AggregateFunction function, string agrregateOn)
        {
            if (type == typeof(int))
            {
                return WithAggregateInfo<int>(propertyName, function, agrregateOn);
            }
            if (type == typeof(long))
            {
                return WithAggregateInfo<long>(propertyName, function, agrregateOn);
            }

            if (type == typeof(float))
            {
                return WithAggregateInfo<float>(propertyName, function, agrregateOn);
            }
            if (type == typeof(double))
            {
                return WithAggregateInfo<double>(propertyName, function, agrregateOn);
            }
            if (type == typeof(decimal))
            {
                return WithAggregateInfo<decimal>(propertyName, function, agrregateOn);
            }

            return this;
        }
        public GroupChildRuleSetBuilder<TEntity, TKey> WithRuleSet(string name,string description="")
        {
            return GroupChildRuleSetBuilder<TEntity, TKey>.Create(_context, name,description,_ruleparser, _propertyparser, this);
        }
        private string EntryCriteriaRuleName()
        {
            return String.Format("EntryCriteria-Ruleset({0})", _name);
        }

        internal void AddRuleSet(IRuleset<TEntity> ruleSet)
        {
            _ruleSets.Add(ruleSet);
        }

        internal void AddRuleSetBuillder(RulesetBuilder<TEntity> ruleSetBuilder) 
        {
            _ruleSetBuilders.Add(ruleSetBuilder);
        }

        public IGroupRuleSet<TEntity> Compile()
        {
            CompilePlaceHolders();
            EntryCriteriaRule<TEntity> entrycriteria = CreateEntryCriteria();
            CompileAggregateProperty();
            CompileChildRuleSet();
            return CreateGroupRuleSet(entrycriteria);

        }

        private void CompilePlaceHolders()
        {
            
            foreach (var item in _placeHolders)
            {
                if (RuleAwareEntityPropertyInfo.HasProperty<TEntity>(_context.RootEntityType, item.Key))
                {
                    throw new PlaceHolderException(item.Key, item.Key, _context.RootEntityType, _context.RuleEngineId);
                    
                }

                _propertyparser.AddPlaceHolder(item.Key, item.Value);
                _ruleparser.AddPlaceHolder(item.Key, item.Value);
                _groupKeyExpressionParser.AddPlaceHolder(item.Key, item.Value);

            }
        }

        private IGroupRuleSet<TEntity> CreateGroupRuleSet(EntryCriteriaRule<TEntity> entrycriteria)
        {
            if (_keySelector != null)
            {
                return new LamdaGroupRuleSet<TEntity, TKey>(_ruleSets, entrycriteria, _aggregateProps, _context, _keySelector, _name, _description);
            }

            if (!string.IsNullOrEmpty(_groupKey))
            {
               var parsedGroupKey= _groupKeyExpressionParser.ParseGroupString(_context, _groupKey);
                return new LamdaGroupRuleSet<TEntity, object>(_ruleSets, entrycriteria, _aggregateProps, _context
                    , t => parsedGroupKey.DynamicInvoke(t),_name, _description);
            }
            return new LamdaGroupRuleSet<TEntity, bool>(_ruleSets, entrycriteria, _aggregateProps, _context, t => true, _name, _description);
          
        }

        private void CompileChildRuleSet()
        {
            foreach (var item in _ruleSetBuilders)
            {
                _ruleSets.Add(item.Compile());
            }
        }

        private void CompileAggregateProperty()
        {
            foreach (var item in _aggregateProps)
            {
                if (_placeHolders.ContainsKey(item.PropertyName))
                    throw new PlaceHolderException(item.PropertyName,_name, _context.RuleEngineId);
                    

                item.ParsePropertyExpression(_placeHolders);
            }
        }

        private EntryCriteriaRule<TEntity> CreateEntryCriteria()
        {
            EntryCriteriaRule<TEntity> entrycriteria = EntryCriteriaRule<TEntity>.TrueRule;
            if (!string.IsNullOrEmpty(_entryCriteria))
            {
                entrycriteria = new EntryCriteriaRule<TEntity>(_context, _entryCriteria, 
                    _ruleparser.ParserRule(_context, _entryCriteria), 
                    EntryCriteriaRuleName());
            }
            else if (_rulefunc != null)
            {
                entrycriteria = new EntryCriteriaRule<TEntity>(_context, EntryCriteriaRuleName(), _rulefunc, EntryCriteriaRuleName());
            }

            return entrycriteria;
        }


        public static GroupRuleSetBuilder<TEntity,TEntity> Create(IRuleEngineContext context)
        {
            return new GroupRuleSetBuilder<TEntity, TEntity>(context as RuleEngineContext.RuleEngineContext);
        }
        public static GroupRuleSetBuilder<TEntity, TKey> Create<TKey>(IRuleEngineContext context)
        {
            return new GroupRuleSetBuilder<TEntity, TKey>(context as RuleEngineContext.RuleEngineContext);
        }
    }
}
