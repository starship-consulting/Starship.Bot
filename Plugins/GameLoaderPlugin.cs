using Starship.Bot.Events;
using Starship.Core.Plugins;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class GameLoaderPlugin : Plugin {

        public override void Start() {
            Process = new WindowsProcess(ProcessName);

            var process = Process.GetProcess();

            if (process != null) {
                Publish(new GameLoaded {
                    Process = Process
                });
            }
        }

        public string ProcessName { get; set; }

        private WindowsProcess Process { get; set; }
    }
}