using System.Data;
using System.Threading;
using System.Threading.Tasks;
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
            
            var thread = new Thread(UpdateLoop);
            thread.IsBackground = true;
            thread.Start();

            /*foreach(var region in e.Data) {
                var model = Regions.GetRegion(region.Name);
                //model.sc
            }*/

            return;

            DarkestDungeonGame.Initialize();

            Planner = new BackwardChainingPlanner<DarkestDungeonViewModel>();
            
            var goal = new Rule<DarkestDungeonViewModel>(model => model.Status == DarkestDungeonGameStatusTypes.InGame);

            var solution = Planner.GetSolution(Model, goal);
        }

        private void UpdateLoop() {
            while(Status == TaskStatus.Running) {
                Model.UpdateState();
                Thread.Sleep(10);
            }
        }

        public DarkestDungeonViewModel Model { get; set; }

        public BackwardChainingPlanner<DarkestDungeonViewModel> Planner { get; set; }
    }
}