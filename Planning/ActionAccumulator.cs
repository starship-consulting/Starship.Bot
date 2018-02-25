using System.Collections.Generic;

namespace Starship.Bot.Planning {
    public abstract class ActionAccumulator {
        public abstract List<PlanAction> GetPossibleActions(StateContainer state);
    }
}