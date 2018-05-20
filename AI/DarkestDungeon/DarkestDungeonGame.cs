using System;
using Starship.Bot.Core;
using Starship.Bot.Models;
using Starship.Bot.Plugins;

namespace Starship.Bot.AI.DarkestDungeon {
    public static class DarkestDungeonGame {

        // Todo:  Initialize via reflection & injection from configuration
        static DarkestDungeonGame() {
            Regions = GameBot.Plugins.Get<RegionPlugin>();
            CampaignButton = Regions.GetRegion("CampaignButton");
        }

        public static RegionViewModel CampaignButton { get; set; }

        private static RegionPlugin Regions { get; set; }
    }
}