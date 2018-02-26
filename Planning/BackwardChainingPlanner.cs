using System;
using System.Collections.Generic;
using System.Reflection;
using Starship.Core.Extensions;
using Starship.Core.Rules;

namespace Starship.Bot.Planning {
    public class BackwardChainingPlanner<T> {

        public BackwardChainingPlanner() {
            Actions = AccumulateActions();
        }

        public Stack<PlanStep> GetSolution(T model, Rule<T> goal) {
            var actions = GetSatisfyingActions(model, goal);
            
            foreach(var action in actions) {
                var steps = new Stack<PlanStep>();
                steps.Push(new PlanStep(action));

                foreach(var step in GetSolution(model, action.Constraint)) {
                    steps.Push(step);
                }

                return steps;
            }

            return new Stack<PlanStep>();
        }

        private List<PlanAction<T>> GetSatisfyingActions(T model, Rule<T> goal) {
            var actions = new List<PlanAction<T>>();

            foreach(var action in Actions) {
                var clone = model.DeepJsonClone();
                action.ApplyExpectedOutcome(clone);

                if(goal.Validate(clone)) {
                    actions.Add(action);
                }
            }

            return actions;
        }

        private static List<PlanAction<T>> AccumulateActions() {
            var assemblies = new List<Assembly> {
                Assembly.GetExecutingAssembly()
            };

            var actions = new List<PlanAction<T>>();

            foreach (var action in typeof(PlanAction<T>).GetTypesOf(assemblies)) {
                actions.Add(Activator.CreateInstance(action) as PlanAction<T>);
            }

            return actions;
        }

        private List<PlanAction<T>> Actions { get; set; }
    }
}