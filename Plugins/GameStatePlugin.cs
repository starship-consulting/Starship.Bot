using Starship.Bot.AI.DarkestDungeon;
using Starship.Bot.Data;
using Starship.Bot.Events;

namespace Starship.Bot.Plugins {
    public class GameStatePlugin : GamePlugin {
        public override void Ready() {
            base.Ready();

            State = new DarkestDungeonGameState();
            On<DataLoaded<Region>>(OnRegionsLoaded);
        }

        private void OnRegionsLoaded(DataLoaded<Region> e) {

            State.IsLoaded = true;
        }

        public DarkestDungeonGameState State { get; set; }
    }
}