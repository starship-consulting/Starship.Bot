using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Starship.Bot.Events;
using Starship.Bot.Models;
using Point = System.Windows.Point;
using Region = Starship.Bot.Data.Region;

namespace Starship.Bot.Plugins {
    public class RegionPlugin : GamePlugin {
        public RegionPlugin() {
            Regions = new List<RegionViewModel>();
        }

        public override void Ready() {
            On<DataLoaded<Region>>(OnRegionsLoaded);
            On<ImageCaptured>(OnImageCaptured);
            Overlay.MouseStateChanged += OnMouseStateChanged;
        }

        public void WithRegion(string name, Action<RegionViewModel> action) {
            var region = GetRegion(name);

            if(region != null) {
                action(region);
            }
        }

        public RegionViewModel GetRegion(string name) {
            name = name.ToLower();
            return Regions.FirstOrDefault(each => each.Name.ToLower() == name);
        }

        private void OnImageCaptured(ImageCaptured e) {
            foreach (var region in Regions) {
                region.Scan(e.Image);
            }
        }

        private void OnRegionsLoaded(DataLoaded<Region> e) {
            foreach (var region in e.Data) {
                var viewmodel = new RegionViewModel(region);
                Regions.Add(viewmodel);

                //viewmodel.Exists();
                viewmodel.AddToUI(Overlay);
            }
        }

        private void OnMouseStateChanged(MouseState e) {
            if (e.IsLeftButtonDown) {
                if (EditingRegion == null) {
                    EditingRegion = new RegionViewModel(e.Position);
                    EditingRegion.AddToUI(Overlay);
                    DragOrigin = e.Position;
                    return;
                }

                EditingRegion.ApplyDrag(DragOrigin, e.Position);
            }
            else {
                CommitRegion();
            }
        }

        private void CommitRegion() {
            if (EditingRegion != null) {
                Regions.Add(EditingRegion);
                EditingRegion.Commit();
                System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(EditingRegion));
                EditingRegion = null;
            }
        }

        public bool IsEnabled { get; set; }

        public List<RegionViewModel> Regions { get; set; }

        private Point DragOrigin { get; set; }

        private RegionViewModel EditingRegion { get; set; }
    }
}