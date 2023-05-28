
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using TestMod.Players;

namespace TestMod.Config {

    public class MainServerConfig : ModConfig {

        
        private static MainServerConfig Instance;
        private bool online = false;

        public override bool AcceptClientChanges(ModConfig _pendingConfig, int whoAmI, ref string message) {
            //message = "Auth Complete";
            var pendingConfig = (MainServerConfig) _pendingConfig;

            Player player = Main.player[whoAmI];
            ConfigPlayer modPlayer = player.GetModPlayer<ConfigPlayer>();
            if (pendingConfig.PasswordOnlyCheat != this.PasswordOnlyCheat) {
                message = "Only the host can change 'Password Required'";
                return false;
            }

            if (!modPlayer.HasAuthenitcated && !TestMod.IsHost(whoAmI)) {
                message = "You are not authorized to change this config";
                return false;
            }
            
            return true ;
        }

        public override void OnLoaded() {
            Instance = this;
            base.OnLoaded();
        }

        public static MainServerConfig GetConfig() {
            //if (Instance == null) {
            //    return Instance =  new();
            //}
            return Instance;
        }

        [Header("Cheat Options")]
        [Label("Admin Password")]
        [Tooltip("The password that is used for cheat options")]
        [DefaultValue("password")]
        public string Password {
            get; set;
        }

        [Label("Password Required")]
        [Tooltip("Cheats with in the mod can only be used by the host and those with the password")]
        [DefaultValue(true)]
        public bool PasswordOnlyCheat {
            get;
            set;
        }

        [Header("Debug")]
        [Label("Test Auth Change")]
        [DefaultValue(false)]
        public bool Debug {
            get; set;
        }

        public ConfigScope GetMode() {
            if (!online) {
                online = true;
                return ConfigScope.ClientSide;
            }
            return ConfigScope.ServerSide;
        }

        public override ConfigScope Mode => GetMode();
    }

}
