//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using Terraria.ModLoader;
//using Terraria.ModLoader.Config;
//using Terraria.ID;

//namespace TestMod.Config {

//    public abstract class ServerConfig : ModType {

//        //public Mod Mod{
//        //    get; private set;
//        //}

//        //public String Name {
//        //    get;
//        //    protected set;
//        //}

//        public string ConfigPath {
//            get; set;
//        } = $"{ConfigManager.ServerModConfigPath}{Path.PathSeparator}";

//        public override string ToString() {
//            return JsonSerializer.Serialize(this, GetType(), options: new() {
//                Converters = {
//                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
//                },
//                AllowTrailingCommas = true,
//                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
//                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//                IncludeFields = true,
//                ReadCommentHandling = JsonCommentHandling.Allow,
//                UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
//                WriteIndented = true,
//            });
//        }

//        protected ServerConfig(string name) {
//            var finstance = this.GetType().GetField("Instance", System.Reflection.BindingFlags.Static) ?? this.GetType().GetField("instance", System.Reflection.BindingFlags.Static);
//            var pinstance = this.GetType().GetProperty("Instance", System.Reflection.BindingFlags.Static) ?? this.GetType().GetProperty("instance", System.Reflection.BindingFlags.Static);
//            if (finstance != null) {
//                finstance.SetValue(null, this);
//                return;
//            }
//            if (pinstance == null) {
//                return;
//            }
//            pinstance.SetValue(null, this);
            
//        }

//        public virtual bool AcceptClientChanges(ServerConfig pendingConfig, int whoAmI, ref string message) {
//            return false;
//        }

//        public virtual void OnLoaded() { }

//        protected sealed override void Register() {
//            ModTypeLookup<ServerConfig>.Register(this);
//        }
//    }
//}
