using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Starship.Azure.Providers;
using Starship.Bot.Data;
using Starship.Bot.Events;
using Starship.Core.Events.Standard;
using Starship.Core.Plugins;
using Starship.Core.Utility;
using Region = Starship.Bot.Data.Region;

namespace Starship.Bot.Plugins {
    public class StoragePlugin : Plugin {

        public StoragePlugin() {
            /*EntityConstructor = new WorkFlow().On<Region>(region => {
                FileProvider.GetContainer(Partition).Download(region.GetImageFilename(), region.GetImagePath());
            });*/
        }

        public override void Ready() {
            base.Ready();

            FileProvider = new AzureBlobProvider(ConnectionString);

            On<EntityModified>(OnEntityModified);
            On<FileModified>(OnFileModified);
        }
        
        protected override void Run() {
            LoadGameBindings();
            //LoadFromAzureTableStorage<Region>();
        }

        private void OnFileModified(FileModified e) {
            using(var stream = new MemoryStream(e.Data)) {
                var image = Image.FromStream(stream);
                image.Save(GetLocalImagePath() + e.Name);
            }
            //FileProvider.Upload(e.Data, Partition, e.Name);
        }

        private string GetLocalImagePath() {
            return Environment.CurrentDirectory + @"..\..\..\Configurations\images\";
        }

        private string GetLocalBindingsPath() {
            return Environment.CurrentDirectory + @"..\..\..\Configurations\bindings.json";
        }

        private Image LoadThumbnail(Region region)  {
            var path = GetLocalImagePath() + region.Id + ".png";

            if(File.Exists(path)) {
                return Image.FromFile(path);
            }

            return null;
        }

        private void SaveGameBindings() {
            if(!EnableSaving) {
                return;
            }

            var json = JsonConvert.SerializeObject(Bindings, Formatting.Indented);
            File.WriteAllText(GetLocalBindingsPath(), json);
        }

        private void LoadGameBindings() {
            var json = File.ReadAllText(GetLocalBindingsPath());
            Bindings = JsonConvert.DeserializeObject<GameBindings>(json);

            if(Bindings != null && Bindings.Regions != null) {

                foreach(var region in Bindings.Regions) {
                    region.Thumbnail = LoadThumbnail(region);
                }

                Publish(new DataLoaded<Region>(Bindings.Regions));
            }
        }

        private void OnEntityModified(EntityModified e) {
            Bindings.Apply(e.Entity);
            SaveGameBindings();
            //SaveToAzureTableStorage(e.Entity);
        }

        private void SaveToAzureTableStorage(object obj) {
            var entity = AzureTableProvider.MakeTableEntity(obj);
            entity.PartitionKey = Partition;

            GetTableContext(obj.GetType().Name).Save(entity);
        }
        
        private void LoadFromAzureTableStorage<T>() where T : class {
            var context = GetTableContext<T>();
            var data = context.All().ToArray();

            var results = AzureTableProvider.Convert<T>(data);

            EntityConstructor.Process(results.ToArray());

            //DataStore.Add(results.ToArray());
            Publish(new DataLoaded<T>(results));
        }

        private AzureTableContext<DynamicTableEntity> GetTableContext<T>() {
            return GetTableContext(typeof(T).Name);
        }

        private AzureTableContext<DynamicTableEntity> GetTableContext(string name) {
            return AzureTableProvider.GetContext<DynamicTableEntity>(name);
        }

        public string Partition { get; set; }

        public string ConnectionString { get; set; }

        public bool EnableSaving { get; set; }
        
        private GameBindings Bindings { get; set; }

        private WorkFlow EntityConstructor { get; set; }

        private AzureBlobProvider FileProvider { get; set; }
    }
}