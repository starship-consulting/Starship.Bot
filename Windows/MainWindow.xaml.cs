using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Starship.Win32.Presentation;

namespace Starship.Bot.Windows {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();

            Instance = this;
            Regions = new List<VisualElement>();
            RegionList.ItemsSource = Regions;
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
                }
                else {
                    region.Unselect();
                }
            }
        }

        public static MainWindow Instance { get; set; }

        private List<VisualElement> Regions { get; set; }
    }
}