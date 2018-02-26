using System;
using Starship.Bot.AI.DarkestDungeon.Data;
using Starship.Bot.Planning;
using Starship.Core.Rules;

namespace Starship.Bot.AI.DarkestDungeon.Interaction {
    public class PlayCampaignButton : PlanAction<DarkestDungeonViewModel> {

        public PlayCampaignButton() {
            Constraint = new Rule<DarkestDungeonViewModel>(model => model.Status == DarkestDungeonGameStatusTypes.ChooseCampaign);
        }

        public override void Execute() {
        }

        public override void ApplyExpectedOutcome(DarkestDungeonViewModel model) {
            model.Status = DarkestDungeonGameStatusTypes.InGame;
        }
    }
}