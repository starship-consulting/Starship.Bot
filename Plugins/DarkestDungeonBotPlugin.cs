using Starship.Bot.AI.DarkestDungeon;
using Starship.Bot.Data;
using Starship.Bot.Events;

namespace Starship.Bot.Plugins {
    public class DarkestDungeonBotPlugin : GamePlugin {

        protected override void OnGameLoaded(GameLoaded e) {
            State = new DarkestDungeonGameState();
            On<DataLoaded<Region>>(OnRegionsLoaded);
        }

        private void OnRegionsLoaded(DataLoaded<Region> e) {
            //Window.BringToFront();
            State.IsLoaded = true;

            /*With<RegionPlugin>(plugin => {
                plugin.WithRegion("NewRegion", region => {
                    GameWindow.Click(region.GetCenter());
                });
            });*/
        }

        public DarkestDungeonGameState State { get; set; }
    }
}