using System;
using Starship.Bot.AI.DarkestDungeon.Data;
using Starship.Bot.Core;
using Starship.Bot.Planning;
using Starship.Bot.Plugins;

namespace Starship.Bot.AI.DarkestDungeon.Actions {
    public class StartCampaign : PlanAction<DarkestDungeonGameState> {

        public override bool ApplyExpectedOutcome(DarkestDungeonGameState state) {
            if(state.Status == DarkestDungeonGameStatusTypes.MainMenu) {
                state.Status = DarkestDungeonGameStatusTypes.ChooseCampaign;
                return true;
            }

            return false;
        }

        public override void Execute() {

            GameBot.With<RegionPlugin>(plugin => {
                plugin.WithRegion("CampaignButton", region => {
                    GameBot.Window.Click(region.GetCenter());
                });
            });
        }
    }
}