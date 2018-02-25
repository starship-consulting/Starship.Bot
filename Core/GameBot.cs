using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Starship.Bot.Interfaces;
using Starship.Core.Extensions;
using Starship.Core.Plugins;
using Starship.Win32;

namespace Starship.Bot.Core {
    public static class GameBot {

        static GameBot() {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            Plugins = new PluginManager(typeof(Plugin).GetTypesOf(assemblies));
        }
        
        public static void Start(IsOverlay overlay, string configuration) {
            Overlay = overlay;
            Plugins.Load(configuration);
            Plugins.Start();
        }

        public static void Stop() {
            Plugins.Stop();
        }

        /*public static void Stop(Action callback) {
            ShutdownCallback = callback;
            Plugins.Stop();
            ShutdownCallback.Invoke();
        }*/

        public static void With<T>(Action<T> action) where T : Plugin {
            Plugins.With(action);
        }

        public static TaskStatus Status { get; set; }

        public static PluginManager Plugins { get; set; }

        public static WindowsProcess Process { get; set; }

        public static WindowInstance Window { get; set; }

        public static IsOverlay Overlay { get; set; }

        private static Action ShutdownCallback { get; set; }
    }
}