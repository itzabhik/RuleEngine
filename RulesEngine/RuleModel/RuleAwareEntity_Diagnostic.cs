using RulesEngine.Diagnostic;

using System.Collections.Generic;
namespace RulesEngine.RuleModel
{
    partial class RuleAwareEntity : IDiagnosticAwareEntity
    {
        private List<string> _diagnosticHolder = new List<string>();

        internal void EntryCriteriaDiagnostic(string entryCriteriaRule, bool isPassed=true)
        {
            string entryCriteriaInformation = string.Format("$Entity_Id: {0} $Entry Criteria: {1} $Status: {2}.",
                Id, entryCriteriaRule, isPassed?"Passed":"Failed");
            _diagnosticHolder.Add(entryCriteriaInformation);
        }

        internal void RulePassedDiagnostic(string rule)
        {
            string ruleInfo = string.Format("->$Entity_Id:{0} $Rule: {1} $Status: {2}.", Id, rule, "Passed");
            _diagnosticHolder.Add(ruleInfo);
        }

        internal void PropertySetDiagnostic(string  propertyName, object propertyValue)
        {
            string propertyInfo = string.Format("-->$Entity_Id:{0} $Property: {1} $Value: {2}.", Id, propertyName, propertyValue);
            _diagnosticHolder.Add(propertyInfo);
        }
        public IEnumerable<string> GetDiagnosticInfo()
        {
            return _diagnosticHolder;
        }
    }
}
