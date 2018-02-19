using System.Collections.Generic;
using System.Linq;

namespace Starship.Bot.Data {
    public class GameBindings {

        public GameBindings() {
            Regions = new List<Region>();
        }

        public void Apply(object entity) {
            if (entity is Region region) {
                var match = Regions.FirstOrDefault(each => each.Id == region.Id);

                if(match != null) {
                    Regions.Remove(match);
                }

                Regions.Add(region);
            }
        }

        public List<Region> Regions { get; set; }
    }
}