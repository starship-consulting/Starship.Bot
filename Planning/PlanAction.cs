using System;
using Starship.Core.Rules;

namespace Starship.Bot.Planning {

    public abstract class PlanAction {
        public abstract void Execute();

        public override string ToString() {
            return GetType().Name;
        }
    }

    public abstract class PlanAction<T> : PlanAction {

        public Rule<T> Constraint { get; set; }

        public abstract void ApplyExpectedOutcome(T model);
    }
}