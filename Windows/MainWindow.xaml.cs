using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Starship.Bot.Configurations;
using Starship.Bot.Core;
using Starship.Bot.Events;
using Starship.Bot.Models;
using Starship.Bot.Plugins;
using Starship.Core.Events;
using Starship.Core.Extensions;
using Starship.Win32.Extensions;
using Starship.Win32.Presentation;

namespace Starship.Bot.Windows {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();

            DataContext = Model = new MainWindowViewModel();

            Instance = this;
            Regions = new ObservableCollection<VisualElement>();
            RegionList.ItemsSource = Regions;

            EventStream.On<FrameRateUpdated>(OnFrameRateUpdated);
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Overlay = new BotOverlay();
            Overlay.Show();

            GameBot.Start(Overlay, BotConfigurations.darkestdungeon.ConvertToString());
        }
        
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            GameBot.Stop();
            Overlay.Close();

            /*e.Cancel = true;
            
            GameBot.Stop(()=> {
                this.UI(()=> {
                    Overlay.Close();
                    Close();
                });
            });*/
        }

        private void OnFrameRateUpdated(FrameRateUpdated e) {
            this.UI(()=> {
                //FPS.Text = "FPS: " + e.FPS;
            });
        }

        public void Add(VisualElement element) {
            if(element is RectangleElement) {
                Regions.Add(element);
            }
        }

        private void RegionList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            foreach(var region in Regions) {
                if (region == RegionList.SelectedItem) {
                    region.Select();
                    
                    Model.Edit(()=> {
                        //var regionModel = GameBot.Plugins.Get<RegionPlugin>().GetRegion(region.Name);

                        Model.ImagePath = GameBot.Plugins.Get<StoragePlugin>().GetLocalImagePath(region.Id.ToString());
                        Model.ImageWidth = region.As<RectangleElement>().Width;
                        Model.ImageHeight = region.As<RectangleElement>().Height;
                    });
                }
                else {
                    region.Unselect();
                }
            }
        }

        public static MainWindow Instance { get; set; }

        private MainWindowViewModel Model { get; set; }

        private ObservableCollection<VisualElement> Regions { get; set; }

        private BotOverlay Overlay { get; set; }
    }
}