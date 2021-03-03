
using RulesEngine.RuleModel;
using RulesEngine.RuleParser;
using System;
using System.Collections.Generic;

namespace RulesEngine.RuleEngineMetadata
{
    public enum AggregateFunction
    {
        Average,
        Count, 
        Max, 
        Min, 
        Sum
    }
    abstract class AggregatePropertyMetadata<TEntity> where TEntity : RuleAwareEntity
    {
        protected  readonly RuleEngineContext.RuleEngineContext _context;
       
        public AggregatePropertyMetadata(RuleEngineContext.RuleEngineContext context, string propertyName,
            AggregateFunction aggregateFunction,Type type, string aggregateOn)
        {
          
            AggregateFunction = aggregateFunction;
            Type = type;
            this._context = context;
            PropertyName = propertyName;
            PropertyExpression = aggregateOn;
           
            
        }

        public AggregateFunction AggregateFunction { get; }
        public Type Type { get; protected set; }
        public string PropertyName { get; }
        public string PropertyExpression { get;  }
        internal Func<TEntity,TType> Parsed<TType>()
        {
            return null;
        }
        internal virtual void ParsePropertyExpression(Dictionary<string, string> placeHolders)
        {
            var prop = new Property(PropertyName, Type, PropertyCategory.Calculated);
            RuleAwareEntityPropertyInfo.CreateCalculatedProperty<TEntity>(_context.RootEntityType, prop);
        }
    }

    class AggregatePropertyMetadataType<TEntity, TType> : AggregatePropertyMetadata<TEntity> where TEntity : RuleAwareEntity
    {
        protected readonly IAggregateFunctionParser<TEntity, TType> _aggregateFunctionParser;

        public AggregatePropertyMetadataType(RuleEngineContext.RuleEngineContext context, string propertyName, AggregateFunction aggregateFunction, 
         string aggregateOn, IAggregateFunctionParser<TEntity, TType> aggregateFunctionParser) 
            : base(context, propertyName, aggregateFunction, typeof(TType), aggregateOn)
        {
            this._aggregateFunctionParser = aggregateFunctionParser;
        }

        public Func<TEntity, TType> ParsedPropertyExpression { get; protected set; }

        internal override void ParsePropertyExpression(Dictionary<string, string> placeHolders)
        {
            _aggregateFunctionParser.AddPlaceHolders(placeHolders);
              ParsedPropertyExpression = _aggregateFunctionParser.ParserRule(_context, PropertyExpression);
            base.ParsePropertyExpression(placeHolders);
        }
    }

    class CountAggregatePropertyMetadataType<TEntity> : AggregatePropertyMetadataType<TEntity,bool> where TEntity : RuleAwareEntity
    {
        public CountAggregatePropertyMetadataType(RuleEngineContext.RuleEngineContext context, string propertyName, Type type, string aggregateOn,
             IAggregateFunctionParser<TEntity, bool> aggregateFunctionParser) 
            : base(context, propertyName, AggregateFunction.Count, aggregateOn, aggregateFunctionParser)
        {
            Type = type;
        }

        internal override void ParsePropertyExpression(Dictionary<string, string> placeHolders)
        {
            if(string.IsNullOrEmpty(PropertyExpression))
            {
                ParsedPropertyExpression = t => true;
                var prop = new Property(PropertyName, Type, PropertyCategory.Calculated);
                RuleAwareEntityPropertyInfo.CreateCalculatedProperty<TEntity>(_context.RootEntityType, prop);
            }
            else
            {
                base.ParsePropertyExpression(placeHolders);
            }
        }
    }
}
