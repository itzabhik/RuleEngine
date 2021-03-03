using System.Collections.Generic;

namespace RulesEngine.Diagnostic
{
    public interface IDiagnosticAwareEntity
    { 
        IEnumerable<string>  GetDiagnosticInfo();
    }
}
