using System.Collections.Generic;

namespace Starship.Bot.Events {
    public class DataLoaded<T> {

        public DataLoaded() {
            Data = new List<T>();
        }

        public DataLoaded(List<T> data) {
            Data = data;
        }

        public List<T> Data { get; set; }
    }
}