using System.Data;
using Starship.Bot.AI.DarkestDungeon;
using Starship.Bot.AI.DarkestDungeon.Data;
using Starship.Bot.Data;
using Starship.Bot.Events;
using Starship.Bot.Planning;
using Starship.Core.Rules;

namespace Starship.Bot.Plugins {
    public class DarkestDungeonBotPlugin : GamePlugin {

        protected override void OnGameLoaded(GameLoaded e) {
            Model = new DarkestDungeonViewModel();
            On<DataLoaded<Region>>(OnRegionsLoaded);
        }

        private void OnRegionsLoaded(DataLoaded<Region> e) {
            Model.IsLoaded = true;

            DarkestDungeonGame.Initialize();

            Planner = new BackwardChainingPlanner<DarkestDungeonViewModel>();
            
            var goal = new Rule<DarkestDungeonViewModel>(model => model.Status == DarkestDungeonGameStatusTypes.InGame);

            var solution = Planner.GetSolution(Model, goal);
        }

        public DarkestDungeonViewModel Model { get; set; }

        public BackwardChainingPlanner<DarkestDungeonViewModel> Planner { get; set; }
    }
}