using Terraria.ModLoader;
using TestMod.Config;
using TestMod.Players;
using TestMod.Utils;

namespace TestMod.Commands {
    public class Auth : ModCommand {
        public override CommandType Type => CommandType.Server;
        public override string Command => "auth";
        public override string Description => "Authenticates You to The Server";

        public override void Action(CommandCaller caller, string input, string[] args) {
            
            try {
                _ = args[0];
            } catch {
                caller.Reply("A password must be specified");
                return;
            }

            string password = args[0];
            for (var i = 1;  i < args.Length; i++) {
                password += " " + args[i];
            }

            MainServerConfig config = MainServerConfig.GetConfig();
            if (password == config.Password) {
                caller.Reply("Login Success");
                caller.Player.GetModPlayer<ConfigPlayer>().HasAuthenitcated = true;
            }
            caller.Reply("Incorrect Password");
        }
    }
}
