using System;

namespace Starship.Bot.Planning {

    public abstract class PlanAction {
    }

    public abstract class PlanAction<T> : PlanAction {

        public abstract bool ApplyExpectedOutcome(T state);

        public abstract void Execute();
    }
}