using System;
using Starship.Bot.AI.DarkestDungeon.Data;
using Starship.Bot.Planning;
using Starship.Core.Rules;

namespace Starship.Bot.AI.DarkestDungeon.Interaction {
    public class CampaignButton : PlanAction<DarkestDungeonViewModel> {

        public CampaignButton() {
            Constraint = new Rule<DarkestDungeonViewModel>(model => model.Status == DarkestDungeonGameStatusTypes.MainMenu);
        }
        
        public override void ApplyExpectedOutcome(DarkestDungeonViewModel model) {
            model.Status = DarkestDungeonGameStatusTypes.ChooseCampaign;
        }

        public override void Execute() {
            DarkestDungeonGame.CampaignButton.Click();
        }
    }
}