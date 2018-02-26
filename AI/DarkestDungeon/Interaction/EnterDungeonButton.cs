using System;
using Starship.Bot.AI.DarkestDungeon.Data;
using Starship.Bot.Planning;
using Starship.Core.Rules;

namespace Starship.Bot.AI.DarkestDungeon.Interaction
{
    public class EnterDungeonButton : PlanAction<DarkestDungeonViewModel> {

        public EnterDungeonButton() {
            Constraint = new Rule<DarkestDungeonViewModel>(model => model.Status == DarkestDungeonGameStatusTypes.DungeonMenu);
        }

        public override void Execute() {
        }

        public override void ApplyExpectedOutcome(DarkestDungeonViewModel model) {
            model.Status = DarkestDungeonGameStatusTypes.InGame;
        }
    }
}
