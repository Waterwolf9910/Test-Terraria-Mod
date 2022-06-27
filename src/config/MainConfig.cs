
using Terraria.ModLoader.Config;

namespace TestMod {

    public class MainConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [JsonDefaultValue("")]
        public bool password {
            get; set;
        }
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) {

            return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
        }
    }
}
