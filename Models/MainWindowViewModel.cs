using System;
using System.Windows.Input;
using Starship.Bot.Events;
using Starship.Bot.Models.Commands;
using Starship.Core.ChangeTracking;
using Starship.Core.Events;

namespace Starship.Bot.Models {
    public class MainWindowViewModel : ChangeTrackedObject {

        public MainWindowViewModel() {
            ScanCommand = new ScanCommandHandler(Scan, true);
        }
        
        public void Scan() {
            EventStream.Publish(new EditModeChanged(true));
        }

        public string ImagePath { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public ICommand ScanCommand { get; set; }
    }
}