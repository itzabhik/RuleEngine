﻿using RulesEngine.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public interface IRuleEngine<TEntity> where TEntity : RuleAwareEntity
    {
        IRuleEngineInstance<TEntity> GetInstance();
    }
}
