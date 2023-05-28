using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using TestMod.Config;
using TestMod.Players;

namespace TestMod.Commands {
    public class CheckAuth : ModCommand {
        public override CommandType Type => CommandType.Server;

        public override string Command => "checkauth";

        public override string Description => "Checks your Auth Status";

        public override void Action(CommandCaller caller, string input, string[] args) {
            ConfigPlayer modPlayer = caller.Player.GetModPlayer<ConfigPlayer>();
            Mod.Logger.Info(MainServerConfig.GetConfig().Debug);
            Mod.Logger.Info(ConfigManager.ModConfigPath);
            
            caller.Reply(modPlayer.HasAuthenitcated ? "Authenticated" : "Not Authenticated");
        }
    }
}
