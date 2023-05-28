using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TestMod.Config;
using TestMod.Utils;

namespace TestMod {
    
    public class TestMod : Mod {

        internal static TestMod Instance;

        //public static event EventHandler<AuthEventArgs> OnAuth;
        public override void Load() {
            this.Logger.Info(ModLoader.ModPath);

            Instance = this;
            base.Load();
        }

        public override void Unload() {
            Instance = null;
            base.Unload();
        }

        public static bool IsHost(int whoAmi) {
            RemoteClient client = Netplay.Clients[whoAmi];
            if (Main.netMode == NetmodeID.SinglePlayer && client.State == 10) {   
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }
            return false;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI) {
            switch ((PacketID)reader.ReadByte()) {
                
                default: {
                    break;
                }
            } 
            base.HandlePacket(reader, whoAmI);
        }
    }
}
