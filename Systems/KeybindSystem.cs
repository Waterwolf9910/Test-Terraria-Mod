using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace TestMod.Systems {

    public class KeybindSystem : ModSystem {

        public static ModKeybind Money {
            get; private set;
        }

        public static ModKeybind OpenCheatMenu {
            get; private set;
        }

        public static ModKeybind CUILeft {
            get; private set;
        }

        public static ModKeybind CUIRight {
            get; private set;
        }

        public static ModKeybind CUIHome {
            get; private set;
        }

        public static ModKeybind CUIEnd {
            get; private set;
        }

        public static ModKeybind Debug1 {
            get; private set;
        }

        public static ModKeybind Debug2 {
            get; private set;
        }

        public override void Load() {

            Money = KeybindLoader.RegisterKeybind(Mod, "Give Money", Keys.K);
            OpenCheatMenu = KeybindLoader.RegisterKeybind(Mod, "Open Cheat Menu", Keys.C);
            Debug1 = KeybindLoader.RegisterKeybind(Mod, "Debug 1", Keys.L);
            Debug2 = KeybindLoader.RegisterKeybind(Mod, "Debug 2", Keys.J);
            CUIHome = KeybindLoader.RegisterKeybind(Mod, "CUI Home", Keys.Home);
            CUIEnd = KeybindLoader.RegisterKeybind(Mod, "CUI End", Keys.End);
            CUILeft = KeybindLoader.RegisterKeybind(Mod, "CUI Left", Keys.Left);
            CUIRight = KeybindLoader.RegisterKeybind(Mod, "CUI Right", Keys.Right);

        }

        public override void Unload() {
            Money = null;
            OpenCheatMenu = null;
        }
    }
}
