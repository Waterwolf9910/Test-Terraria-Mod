using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TestMod.UI.Common;
using TestMod.UI.States;

namespace TestMod.UI.Systems {

    class CheatUISystem : ModSystem {

        internal UserInterface CheatInterface;

        internal CheatUIState UIState;

        internal bool IsOpen => CheatInterface?.CurrentState != null;

        public override void Load() {

            if (Main.dedServ) {
                return;
            }

            UIState = new CheatUIState();
            UIState.Activate();
            CheatInterface = new UserInterface();
        }

        public override void Unload() {
            UIState?.Unload();
            UIState = null;
        }

        public void Redraw() {
            UIState?.Redraw();
        }

        private GameTime _gametime;

        public override void UpdateUI(GameTime gameTime) {
            
            _gametime = gameTime;
            if (Main.keyState.IsKeyDown(Keys.Escape)) {
                this.Hide();
                return;
            }
            if (CheatInterface?.CurrentState == null) {
                return;
            }
            CheatInterface.Update(gameTime);
        }

        public override void PostUpdateInput() {
            if (Main.dedServ) {
                return;
            }
            base.PostUpdateInput();
            if (CheatInterface?.CurrentState is BaseUIState state && state != null) {
                state.PostUpdateInput(_gametime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (mouseTextIndex == -1) {
                return;
            }

            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("TestMod: CheatInterface", () => {
                if (_gametime != null && CheatInterface?.CurrentState != null) {
                    CheatInterface.Draw(Main.spriteBatch, _gametime);
                }
                return true;
            }, InterfaceScaleType.UI));

            //layers.Add(new LegacyGameInterfaceLayer("TestMod: CheatInterface", () => {
            //    if (_gametime != null && CheatInterface?.CurrentState != null) {
            //        CheatInterface.Draw(Main.spriteBatch, _gametime);
            //    }
            //    return true;
            //}, InterfaceScaleType.UI));
        }

        public override void AddRecipeGroups() {
            base.AddRecipeGroups();
            this.UIState.ReloadItems();
        }

        internal void Show() {
            CheatInterface?.SetState(UIState);
        }

        internal void Hide() {
            CheatInterface?.SetState(null);
        }

        internal void Toggle() {
            if (CheatInterface?.CurrentState != null) {
                CheatInterface?.SetState(null);
                return;
            }
            CheatInterface?.SetState(UIState);
        }

        internal void TurnLeft() {
            this.UIState?.TurnLeft();
        }

        internal void TurnRight() {
            this.UIState?.TurnRight();
        }

        internal void Home() {
            this.UIState?.SetPage(0);
        }

        internal void End() {
            this.UIState?.SetPage(this.UIState.SlotAmount / this.UIState.ItemsPer);
        }
    }
}
