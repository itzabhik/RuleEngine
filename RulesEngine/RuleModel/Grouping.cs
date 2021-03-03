using RulesEngine.RuleEngineMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace RulesEngine.RuleModel
{
    class Grouping<TEntity>
        where TEntity : RuleAwareEntity
    {
        private readonly string _entityType;
        private readonly IEnumerable<AggregatePropertyMetadata<TEntity>> _aggregateProps;
        private PropertyBag _aggregateProperties;
        List<string> _allProperties = new List<string>();
      
        public Grouping(RuleEngineContext.RuleEngineContext context, IEnumerable<TEntity> entity, IEnumerable<AggregatePropertyMetadata<TEntity>> aggregateProps)
        {
          
            _aggregateProperties = new PropertyBag( BagType.Calculated, context.RootEntityType);
            _entityType = context.RootEntityType;
            Entity = entity;
            this._aggregateProps = aggregateProps;
            foreach (var item in aggregateProps)
            {
                var prop = RuleAwareEntityPropertyInfo.GetCalculatedProperty(_entityType, item.PropertyName);
                _aggregateProperties.Initialize(prop);
                _allProperties.Add(item.PropertyName);
            }

            CalculateAggregatePropertiesForGroup(entity);
        }

        public IEnumerable<TEntity> Entity { get; }

        private void CalculateAggregatePropertiesForGroup(IEnumerable<TEntity> grouping)
        {
            foreach (var item in _aggregateProps)
            {
                CalculateAggregates(grouping, item);
            }
        }

        public void AttachAggregatePropertyToEntity(TEntity entity)
        {
            foreach (var item in _allProperties)
            {
                entity.InitializeCalculatedProperty(RuleAwareEntityPropertyInfo.GetCalculatedProperty(_entityType, item)
                    , _aggregateProperties[item]);
            }
        }

        private void CalculateAggregates(IEnumerable<TEntity> grouping, AggregatePropertyMetadata<TEntity> item)
        {
            if(item.AggregateFunction==AggregateFunction.Count)
            {
                var countFunction = item as CountAggregatePropertyMetadataType<TEntity>;
                _aggregateProperties[countFunction.PropertyName] = grouping.Count(t => countFunction.ParsedPropertyExpression(t));
                return;
            }
            
            if (item.Type ==typeof(int))
            {
                var intAggr= item as AggregatePropertyMetadataType<TEntity,int>;
                CalculateAggregatesForInt(grouping, intAggr.ParsedPropertyExpression, intAggr.AggregateFunction, intAggr.PropertyName);
                return;
            }
            if (item.Type == typeof(float))
            {
                var intAggr = item as AggregatePropertyMetadataType<TEntity, float>;
                CalculateAggregatesForFloat(grouping, intAggr.ParsedPropertyExpression, intAggr.AggregateFunction, intAggr.PropertyName);
                return;
            }

            if (item.Type == typeof(long))
            {
                var intAggr = item as AggregatePropertyMetadataType<TEntity, long>;
                CalculateAggregatesForLong(grouping, intAggr.ParsedPropertyExpression, intAggr.AggregateFunction, intAggr.PropertyName);
                return;
            }
            if (item.Type == typeof(decimal))
            {
                var intAggr = item as AggregatePropertyMetadataType<TEntity, decimal>;
                CalculateAggregatesForDecimal(grouping, intAggr.ParsedPropertyExpression, intAggr.AggregateFunction, intAggr.PropertyName);
                return;
            }

            if (item.Type == typeof(double))
            {
                var intAggr = item as AggregatePropertyMetadataType<TEntity, double>;
                CalculateAggregatesForDouble(grouping, intAggr.ParsedPropertyExpression, intAggr.AggregateFunction, intAggr.PropertyName);
                return;
            }


        }

        private void CalculateAggregatesForInt(IEnumerable<TEntity> grouping, Func<TEntity,int> item, AggregateFunction function, string propertyName)
        {
            
            switch (function)
            {
                case AggregateFunction.Average:
                    _aggregateProperties[propertyName] = grouping.Average(t => item(t));
                    break;
                case AggregateFunction.Max:
                    _aggregateProperties[propertyName] = grouping.Max(t => item(t));
                    break;
                case AggregateFunction.Min:
                    _aggregateProperties[propertyName] = grouping.Min(t => item(t));
                    break;
                case AggregateFunction.Sum:
                    _aggregateProperties[propertyName] = grouping.Sum(t => item(t));
                    break;
            }

        }

        private void CalculateAggregatesForLong(IEnumerable<TEntity> grouping, Func<TEntity,long> item, AggregateFunction function, string propertyName)
        {

            switch (function)
            {
                case AggregateFunction.Average:
                    _aggregateProperties[propertyName] = grouping.Average(t => item(t));
                    break;
                case AggregateFunction.Max:
                    _aggregateProperties[propertyName] = grouping.Max(t => item(t));
                    break;
                case AggregateFunction.Min:
                    _aggregateProperties[propertyName] = grouping.Min(t => item(t));
                    break;
                case AggregateFunction.Sum:
                    _aggregateProperties[propertyName] = grouping.Sum(t => item(t));
                    break;


            }

        }

        private void CalculateAggregatesForFloat(IEnumerable<TEntity> grouping, Func<TEntity, float> item, AggregateFunction function, string propertyName)
        {

            switch (function)
            {
                case AggregateFunction.Average:
                    _aggregateProperties[propertyName] = grouping.Average(t => item(t));
                    break;
                case AggregateFunction.Max:
                    _aggregateProperties[propertyName] = grouping.Max(t => item(t));
                    break;
                case AggregateFunction.Min:
                    _aggregateProperties[propertyName] = grouping.Min(t => item(t));
                    break;
                case AggregateFunction.Sum:
                    _aggregateProperties[propertyName] = grouping.Sum(t => item(t));
                    break;
            }

        }

        private void CalculateAggregatesForDouble(IEnumerable<TEntity> grouping, Func<TEntity, double> item, AggregateFunction function, string propertyName)
        {

            switch (function)
            {
                case AggregateFunction.Average:
                    _aggregateProperties[propertyName] = grouping.Average(t => item(t));
                    break;
                case AggregateFunction.Max:
                    _aggregateProperties[propertyName] = grouping.Max(t => item(t));
                    break;
                case AggregateFunction.Min:
                    _aggregateProperties[propertyName] = grouping.Min(t => item(t));
                    break;
                case AggregateFunction.Sum:
                    _aggregateProperties[propertyName] = grouping.Sum(t => item(t));
                    break;


            }

        }

        private void CalculateAggregatesForDecimal(IEnumerable<TEntity> grouping, Func<TEntity, decimal> item, AggregateFunction function, string propertyName)
        {

            switch (function)
            {
                case AggregateFunction.Average:
                    _aggregateProperties[propertyName] = grouping.Average(t => item(t));
                    break;
                case AggregateFunction.Max:
                    _aggregateProperties[propertyName] = grouping.Max(t => item(t));
                    break;
                case AggregateFunction.Min:
                    _aggregateProperties[propertyName] = grouping.Min(t => item(t));
                    break;
                case AggregateFunction.Sum:
                    _aggregateProperties[propertyName] = grouping.Sum(t => item(t));
                    break;


            }

        }
    }
}
