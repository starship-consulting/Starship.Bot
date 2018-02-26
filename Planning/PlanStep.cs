using System;

namespace Starship.Bot.Planning {
    public class PlanStep {
        
        public PlanStep() {
        }

        public PlanStep(PlanAction action) {
            Action = action;
            Quantity = 1;
        }

        public override string ToString() {
            return Action.ToString();
        }

        public int Quantity { get; set; }

        public PlanAction Action { get; set; }
    }
}