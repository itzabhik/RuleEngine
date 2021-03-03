using RulesEngine.RuleEngineExceptions;
using RulesEngine.RuleEngineContext;
using RulesEngine.RuleExecutionRule;
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using RulesEngine.Rules;
using System;
using System.Collections.Generic;
using RulesEngine.RuleEngineMetadata;

namespace RulesEngine.Ruleset
{
    public class RulesetBuilder<TEntity> where TEntity : RuleAwareEntity
    {
        internal string _name = Guid.NewGuid().ToString();
        private string _description = string.Empty;

        private RuleExecutionRuleEnum _rulesExecutionRule = RuleExecutionRuleEnum.FirstMatch;

        private List<PropertyRuleBuilder<TEntity>> _propertyRuleBuilders = new List<PropertyRuleBuilder<TEntity>>();
        private DefaultRuleBuilder<TEntity> _defaultPropertySetRuleBuilder = null;
        private List<IPropertyRule<TEntity>> _rules = new List<IPropertyRule<TEntity>>();

        private IPropertyRuleExpressionParser<TEntity> _propertyparser;
        private IRuleExpressionParser<TEntity> _ruleparser = null;

        private Dictionary<string, string> _placeHolders = new Dictionary<string, string>();

        private string _entryCriteria;
        private Func<TEntity, bool> _entryCriteriaRulefunc;
        private bool _isEntryCriteriaSet=false;

        internal RuleEngineContext.RuleEngineContext _context;
       
        private RulesetBuilder(RuleEngineContext.RuleEngineContext context )
        {

            _context = context;

            _ruleparser = RuleExpressionParserFactory.CreateStringRuleParser<TEntity>();
            _propertyparser = RuleExpressionParserFactory
                 .CreateStringPropertyRuleParser<TEntity>();
        }
        public RulesetBuilder<TEntity> WithName(string name)
        {
            name = name ?? Guid.NewGuid().ToString();
            _name = name;
            return this;
        }

        public RulesetBuilder<TEntity> WithDescription(string description)
        {
            _description = description ?? string.Empty;
            return this;
        }

        public RulesetBuilder<TEntity> WithPlaceHolder(string placeHolderName, string placeHolderExpression)
        {
            if (_placeHolders.ContainsKey(placeHolderName))
            {
                throw new PlaceHolderException(placeHolderName, _name,_context.RuleEngineId);
                
            }
            _placeHolders.Add(placeHolderName, string.Format("{0}{1}{2}", "(", placeHolderExpression, ")"));

            return this;
        }


        public RulesetBuilder<TEntity> WithEntryCriteria(string entryCriteria)
        {
            CheckEntryCriteriaIsSet();
            _entryCriteria = entryCriteria;
            return this;
        }

       
        public RulesetBuilder<TEntity> WithEntryCriteria(Func<TEntity, bool> rulefunc)
        {
            CheckEntryCriteriaIsSet();
            _isEntryCriteriaSet = true;
            _entryCriteriaRulefunc = rulefunc;
            return this;
        }

        public RulesetBuilder<TEntity> WithJobExecutionRule(RuleExecutionRuleEnum rulesExecutionRule)
        {
            this._rulesExecutionRule = rulesExecutionRule;
            return this;
        }

        public PropertyRuleBuilder<TEntity> WithRule(string rule)
        {
            return WithRule(rule, Guid.NewGuid().ToString(), string.Empty);
        }
        public PropertyRuleBuilder<TEntity> WithRule(string rule, string name, string description = "")
        {
            if (string.IsNullOrEmpty(rule))
                throw new ArgumentNullException(rule, string.Format("Rule is provided as Null for entity Type {0} in Rule Engine {1}", 
                    _context.RootEntityType, _context.RuleEngineId));

            var ruleBuilder = PropertyRuleBuilder<TEntity>.Create(_context, rule, _ruleparser, _propertyparser, this, name, description);
            _propertyRuleBuilders.Add(ruleBuilder);
            return ruleBuilder;
        }
        public PropertyRuleBuilder<TEntity> WithRule(Func<TEntity, bool> rule)
        {
            return WithRule(rule, Guid.NewGuid().ToString(), string.Empty);
        }
        public PropertyRuleBuilder<TEntity> WithRule(Func<TEntity, bool> rule, string name, string description = "")
        {
            if (rule == null)
                throw new ArgumentNullException("rule", string.Format("Rule is provided as Null for entity Type {0} in Rule Engine {1}",
                    _context.RootEntityType, _context.RuleEngineId));

            var ruleBuilder = PropertyRuleBuilder<TEntity>
                .Create(_context, rule, _propertyparser, this, name, description);
            _propertyRuleBuilders.Add(ruleBuilder);
            return ruleBuilder;
        }

        public DefaultRuleBuilder<TEntity> WithDefaultRule()
        {
            if (_rulesExecutionRule != RuleExecutionRuleEnum.FirstMatch)
            {
                throw new ArgumentException
                    (string.Format("Default rule can be only attached when RuleExecutionRule is of type firstmatch in Rule Engine {0}", _context.RuleEngineId));
            }
            var ruleBuilder = DefaultRuleBuilder<TEntity>.Create(_context, this, _propertyparser);
            _defaultPropertySetRuleBuilder = ruleBuilder;
            return ruleBuilder;
        }
      
        internal RulesetBuilder<TEntity> SetPropertyRuleParser(IPropertyRuleExpressionParser<TEntity> propertyparser)
        {
            _propertyparser = propertyparser;
            return this;
        }

        internal RulesetBuilder<TEntity> SetRuleParser(IRuleExpressionParser<TEntity> ruleparser)
        {
            _ruleparser = ruleparser;
            return this;
        }
        public IRuleset<TEntity> Compile()
        {
            CompilePlaceHolders();
            EntryCriteriaRule<TEntity> entrycriteria = CreateEntryCriteria();
            CreateRules();
            CreateDefaultRule();
            return new Ruleset<TEntity>
                (_context, _rules, entrycriteria, _rulesExecutionRule, _name, _description);
        }

        private void CompilePlaceHolders()
        {
            foreach (var item in _placeHolders)
            {
              
                if (RuleAwareEntityPropertyInfo.HasProperty<TEntity>(_context.RootEntityType, item.Key))
                {
                    throw new PlaceHolderException(item.Key, item.Key, _context.RootEntityType,_context.RuleEngineId);
                }

                _propertyparser.AddPlaceHolder(item.Key, item.Value);
                _ruleparser.AddPlaceHolder(item.Key, item.Value);

            }
        }

        private void CreateDefaultRule()
        {
            if (_defaultPropertySetRuleBuilder != null)
            {
                _rules.Add(_defaultPropertySetRuleBuilder.CompileInternal());
            }
        }

        private void CreateRules()
        {
            foreach (var item in _propertyRuleBuilders)
            {
                _rules.Add(item.Compile());
            }
        }

        private EntryCriteriaRule<TEntity> CreateEntryCriteria()
        {
            EntryCriteriaRule<TEntity> entrycriteria = EntryCriteriaRule<TEntity>.TrueRule;
            if (!string.IsNullOrEmpty(_entryCriteria))
            {

                entrycriteria = new EntryCriteriaRule<TEntity>(_context, _entryCriteria, 
                    _ruleparser.ParserRule(_context, _entryCriteria), EntryCriteriaRuleName());
            }
            else if (_entryCriteriaRulefunc != null)
            {
                entrycriteria = new EntryCriteriaRule<TEntity>(_context, EntryCriteriaRuleName(), _entryCriteriaRulefunc, EntryCriteriaRuleName());
            }

            return entrycriteria;
        }

        private void CheckEntryCriteriaIsSet()
        {
            if (_isEntryCriteriaSet)
                throw new ArgumentException(string.Format("Trying to set deuplicate Entry Criteria for Entity Type {0} in RuleEngine {1}"
                    ,_context.RootEntityType, _context.RuleEngineId));
            _isEntryCriteriaSet = true;
        }


        public static RulesetBuilder<TEntity> Create(IRuleEngineContext context)
        {
            if (context==null)
                throw new ArgumentNullException(nameof(context), "Cannot be null");
            return new RulesetBuilder<TEntity>(context as RuleEngineContext.RuleEngineContext);
        }

        private string EntryCriteriaRuleName()
        {
            return String.Format("EntryCriteria-Ruleset({0})", _name);
        }


    }
}
