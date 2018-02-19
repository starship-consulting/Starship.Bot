using Starship.Core.AI;
using Starship.Core.AI.Planning;

namespace Starship.Bot.AI.DarkestDungeon.Actions {
    public class UseItem : PlanAction {

        public override void Simulate(StateContainer state) {
        }

        public override void Execute() {
        }
        
        public GameItem Item { get; set; }

        public GameCharacter Target { get; set; }
    }
}