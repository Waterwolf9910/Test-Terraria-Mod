using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using TestMod.Config;
using TestMod.Systems;
using TestMod.UI.Systems;

namespace TestMod.Players {

    [Autoload(Side = ModSide.Client)]
    public class KeybindPlayer : ModPlayer {

        public override void ProcessTriggers(TriggersSet triggersSet) {
            var cui = ModContent.GetInstance<CheatUISystem>();
            if (KeybindSystem.Money.JustPressed) {
            //this.Player.
                
                this.Player.QuickSpawnItemDirect(new EntitySource_Misc("Coin Get"), MainClientConfig.Instance.CoinType.CTypeToItemID(), MainClientConfig.Instance.CoinAmt);
            }
            if (KeybindSystem.OpenCheatMenu.JustPressed) {
                cui.Toggle();
            }

            if (KeybindSystem.Debug1.JustPressed) {
                cui.Redraw();
            }

            if (KeybindSystem.Debug2.JustPressed) {
            }

            if (KeybindSystem.CUILeft.JustPressed) {
                cui.TurnLeft();
            }

            if (KeybindSystem.CUIRight.JustPressed) {
                cui.TurnRight();
            }

            if (KeybindSystem.CUIHome.JustPressed) {
                cui.Home();
            }

            if (KeybindSystem.CUIEnd.JustPressed) {
                cui.End();
            }

        }

    }
}
