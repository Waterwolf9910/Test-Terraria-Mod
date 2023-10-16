
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace TestMod.Utils {
    public static class Util {

        public static bool IsKeyDown(Keys key) {
            if (Main.dedServ) {
                return false;
            }

            return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
        }

    }
}
