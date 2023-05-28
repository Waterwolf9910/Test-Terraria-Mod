using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TestMod.Config {
    public class MainClientConfig: ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static MainClientConfig Instance;
        public override void OnLoaded() {
            //Instance = this;
            base.OnLoaded();
        }

        //public static MainClientConfig GetConfig() {
        //    //if (Instance == null) {
        //    //    return Instance = new();
        //    //}
        //    return Instance;
        //}


        [Header("Cheat Options")]
        [Label("Coin Amount")]
        [Tooltip("The amount of coins to give with coin giver")]
        [DefaultValue(5)]
        public int CoinAmt {
            get; set;
        }

        [Label("Coin Type")]
        [Tooltip("The type of coin to give")]
        [DefaultValue(Properties.CType.Silver)]
        public Properties.CType CoinType {
            get; set;
        }

        [Header("GUI Settings")]
        [Tooltip("Cheat UI Height")]
        [DefaultValue(500)]
        public int CUIHeight {
            get; set;
        }

        [Tooltip("Cheat UI Width")]
        [DefaultValue(500)]
        public int CUIWidth {
            get; set;
        }

    }

    public static class Properties {
        
        public enum CType {
            Copper,
            Silver,
            Gold,
            Platinum
        }

        public static int CTypeToItemID(this CType type) {
            if (type == CType.Copper) {
                return ItemID.CopperCoin;
            }

            if (type == CType.Gold) {
                return ItemID.GoldCoin;
            }

            if (type == CType.Platinum) {
                return ItemID.PlatinumCoin;
            }

            return ItemID.SilverCoin;
        }
    }
}
