using Starship.Bot.Data;
using Starship.Bot.Events;

namespace Starship.Bot.Plugins {
    public class DarkestDungeonBotPlugin : GamePlugin {

        protected override void OnGameLoaded(GameLoaded e) {
            On<DataLoaded<Region>>(OnRegionsLoaded);
        }

        private void OnRegionsLoaded(DataLoaded<Region> e) {
            GameWindow.BringToFront();

            /*With<RegionPlugin>(plugin => {
                plugin.WithRegion("NewRegion", region => {
                    GameWindow.Click(region.GetCenter());
                });
            });*/
        }
    }
}