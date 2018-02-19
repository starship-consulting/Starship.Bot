using System.Collections.Generic;
using System.Reflection;
using Starship.Bot.Interfaces;
using Starship.Core.Extensions;
using Starship.Core.Plugins;

namespace Starship.Bot.Core {
    public static class GameBot {

        static GameBot() {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            Plugins = new PluginManager(typeof(Plugin).GetTypesOf(assemblies));
        }
        
        public static void Start(IsOverlay ui, string configuration) {
            UI = ui;

            Plugins.Load(configuration);
            Plugins.Start();
        }

        public static PluginManager Plugins { get; set; }

        public static IsOverlay UI { get; set; }
    }
}