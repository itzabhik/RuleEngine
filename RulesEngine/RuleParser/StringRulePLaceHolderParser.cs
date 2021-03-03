using System;
using System.Collections.Generic;
using System.Linq;
using RulesEngine.RuleEngineContext;
using RulesEngine.RuleEngineMetadata;
using RulesEngine.RuleModel;

namespace RulesEngine.RuleParser
{
    internal static class StringRulePLaceHolderParser
    {
        internal static string ParseRulePlaceHolder<TEntity>(RuleEngineContext.RuleEngineContext context, string ruleExpression, 
            Dictionary<string, string> customplaceholders)
            where TEntity : RuleAwareEntity
        {
           
            PlaceHolderTextParser place = new PlaceHolderTextParser(ruleExpression);

            string placeHolder;
            string placeHolderWithParenthesis;

            while (place.NextPlaceHolder(out placeHolder, out placeHolderWithParenthesis))
            {
                ParseRule<TEntity>(context.RootEntityType, context.EntityContextMetadata, ref ruleExpression, ref place, placeHolder, customplaceholders);
            }

            return ruleExpression;
        }
        private static void ParseRule<TEntity>(string entityType, RuleEntityContextMetadata ruleEntityMetadata,
           ref string ruleExpression,
           ref PlaceHolderTextParser place, string placeHolder, Dictionary<string, string> customplaceholders) where TEntity : RuleAwareEntity
        {
            string replacedString = "";
            bool isDynamicProperty;

            if (placeHolder.Contains("."))
            {
                if(TryParseComplexProperty(entityType, ruleEntityMetadata, placeHolder, typeof(TEntity), customplaceholders,
                    out replacedString))
                {
                    ruleExpression = Replace(ruleExpression, place.startplaceHolderpos, place.endPlaceHolderpos, replacedString);
                }
            }
            else if (TryParseSimpleProperty(entityType, ruleEntityMetadata, placeHolder, typeof(TEntity), out replacedString, out isDynamicProperty
                , customplaceholders))
            {
                ruleExpression = Replace(ruleExpression, place.startplaceHolderpos, place.endPlaceHolderpos,
                   replacedString);

            }
            else
            {
                ruleExpression = Replace(ruleExpression, place.startplaceHolderpos, place.endPlaceHolderpos
                    , placeHolder);
            }
            place = new PlaceHolderTextParser(ruleExpression);
        }


        private static bool TryParseSimpleProperty(string entityType, RuleEntityContextMetadata ruleEntityMetadata,
            string property, Type type,
            out string replacedString, out bool resolvedAsDynamicProperty, Dictionary<string, string> customplaceholders, 
            bool resolveInstanceProperty = false)

        {
            resolvedAsDynamicProperty = false;
            replacedString = string.Empty;

            if (TryParseRuleEnginePlaceHolders(customplaceholders, property, out replacedString))
            {
                return true; 
            }

            string alias;
            string propertyWithoutAlias;

            if (TryGetAlias(property,out alias,out propertyWithoutAlias))
            {
                replacedString = property = propertyWithoutAlias;

                if (!TryGetEntityTypeFromAlias(out entityType, ruleEntityMetadata, out type, alias))
                {
                    return true;
                }
            }

            Property prop = RuleAwareEntityPropertyInfo.GetProperty(entityType, property, type);

            if (prop == Property.PropertyNotFound)
            {
                return false;
            }
            if (prop.IsDynamicType || prop.IsCalculated)
            {
                resolvedAsDynamicProperty = true;
                string resolvedString = prop.ResolveRuleDynamicContext();
                if (string.IsNullOrEmpty(resolvedString))
                    return false;

                replacedString = resolvedString;
            }
            if (prop.IsInstanceType)
            {
                replacedString = resolveInstanceProperty ? prop.ResolveRuleDynamicContext() : property;
            }
            return true;
        }

       

        private static bool TryParseComplexProperty(string entityType, RuleEntityContextMetadata ruleEntityMetadata,
            string property, Type type, 
            Dictionary<string, string> customplaceholders, out string replacedString)

        {
            replacedString = string.Empty;
            string workingEntityType = entityType;
            Type workingType = type;

            List<string> lstExpressions = new List<string>();
            bool expressionHasDynamicProperty = false;
            foreach (var prop in property.Split('.'))
            {
                bool isDynamicProperty;

                if (IsFunction(prop))
                {
                    lstExpressions.Add(prop);
                    continue;
                }
                if (TryParseSimpleProperty(workingEntityType, ruleEntityMetadata, prop, workingType, out replacedString, out isDynamicProperty,
                    customplaceholders,
                     expressionHasDynamicProperty))
                {
                    if (isDynamicProperty)
                        expressionHasDynamicProperty = true;
                    lstExpressions.Add(replacedString);
                }
                else
                {
                    lstExpressions.Add(prop);
                }

                RecalculateWorkingEntityType(entityType, ruleEntityMetadata, ref workingEntityType, ref workingType, prop);

            }

            replacedString =string.Join(".", lstExpressions);
            return true;
        }

        private static void RecalculateWorkingEntityType
            (string entityType, RuleEntityContextMetadata ruleEntityMetadata, ref string workingEntityType, ref Type workingType, string property)
        {
            string alias;
            string propertyWithoutAlias;

            string calcProp;
            string calcEntType;
            if (TryGetAlias(property, out alias, out propertyWithoutAlias))
            {
                calcEntType = ruleEntityMetadata.GetDynamicTypeFromAlias(alias);
                calcProp = propertyWithoutAlias;
            }
            else
            {
                calcProp = property;
                calcEntType = entityType;
            }

            var childType = ruleEntityMetadata.GetChildDynamicType(calcEntType, calcProp);
            if (childType != null)
            {
                workingEntityType = childType.EntityType;
                workingType = childType.ChildType;
            }
        }

        private static bool TryParseRuleEnginePlaceHolders(Dictionary<string, string> customplaceholders,
            string property, out string replacedString)
        {
            replacedString = string.Empty;

            if (customplaceholders.ContainsKey(property))
            {
                replacedString = customplaceholders[property];
                return true; ;
            }
            return false;
        }
        private static bool TryGetAlias(string porperty, out string alias, out string propertyWithoutAlias)
        {
            alias = string.Empty;
            propertyWithoutAlias = porperty;
            if (!porperty.Contains(":"))
                return false;
            var vallues = porperty.Split(new[] { ':' });

            alias = vallues[0];
            propertyWithoutAlias = vallues[1];

            return true;
        }

        private static bool TryGetEntityTypeFromAlias(out string entityType, RuleEntityContextMetadata ruleEntityMetadata, out Type type, string alias)
        {
            type = null;

            entityType = ruleEntityMetadata.GetDynamicTypeFromAlias(alias);
            if (string.IsNullOrEmpty(entityType))
                return false;
            type = ruleEntityMetadata.GetTypeForEntityType(entityType);

            return true;

        }

        private static bool IsFunction(string prop)
        {
            return prop.Trim().EndsWith(")");
        }

       

        private static string Replace(string expression, int startIndex, int endIndex, string placeHolder)
        {
            return expression.Remove(startIndex, (endIndex - startIndex) + 1)
                .Insert(startIndex, placeHolder);
        }

    }
}
