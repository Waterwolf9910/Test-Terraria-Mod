using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using TestMod.UI.Common;
using Terraria.GameContent.UI.Elements;
using TestMod.Config;
using TestMod.UI.Elements;

namespace TestMod.UI.States {
    internal class CheatUIState: BaseUIState {

        private List<CheatItemSlot> Slots = new();
        private List<string> ItemNames = new();
        private DraggableUIPanel Panel;
        private SearchBar SearchBar;
        private int lastpage = 0;
        
        public int ItemsPer {
            get;
            private set;
        }

        public int SlotAmount => Slots.Count;

        public void Unload() {
            Panel = null;
            SearchBar = null;
            Slots.Clear();
            ItemNames.Clear();
        }

        public void ReloadItems() {
            for (var i = 1; i < TextureAssets.Item.Length; i++) {
                var slot = new CheatItemSlot(i);
                if (!string.IsNullOrWhiteSpace(slot.GetItemName())) {
                    Slots.Add(slot);
                    ItemNames.Add(slot.GetItemModName());
                }
            }
            
            this.Redraw();
        }

        public void OnSort(string filter) {
            this.Redraw(this.lastpage, filter);
            Console.WriteLine(filter);
        }

        private bool Redrawing = false;
        public void Redraw(int id = 0, string filter = "") {
            this.Redrawing = true;
            //this.Panel.Remove();
            //Panel = new DraggableUIPanel() {
            //    HAlign = 0.5f,
            //    VAlign = 0.5f,
            //};
            Panel.RemoveAllChildren();
            var height = MainClientConfig.Instance.CUIHeight;
            var width = MainClientConfig.Instance.CUIWidth;
            Panel.Height.Set(height, 0);
            Panel.Width.Set(width, 0);
            Panel.SetPadding(0);

            SearchBar.HAlign = 0f;
            SearchBar.Top.Set(height * .02f, 0);
            SearchBar.Left.Set(240, 0);
            //SearchBar.Top.Set(width * -0.05f, 0);
            SearchBar.Width.Set(width / 2, 0);
            Panel.Append(SearchBar);

            UIText text = new("Cheat Menu");
            text.Left.Set(20, 0);
            text.Top.Set(height * .027f, 0);
            Panel.Append(text);

            var spacing = height * .019f;
            var y = height - SearchBar.Height.Pixels - spacing;
            //var y = height - text.Height.Pixels - spacing;
            var yoffset = SearchBar.Top.Pixels + SearchBar.Height.Pixels + spacing;
            //var yoffset = spacing + text.Height.Pixels + spacing * 2;
            var slot = CheatItemSlot.Default;
            filter = string.IsNullOrWhiteSpace(filter) ? _old_filter : filter;
            var _slots = this.Slots.FindAll((_slot) => _slot.GetItemName().ToLower().StartsWith(filter));
            ItemsPer = 0;
            while (y > slot.Height.Pixels + spacing) {
                float x = MainClientConfig.Instance.CUIWidth - spacing;
                float xoffset = spacing;
                //float xoffset = spacing + text.Width.Pixels + 18;
                while (x > slot.Width.Pixels) {
                    ++ItemsPer;
                    x -= slot.Width.Pixels + spacing;
                    if (id >= _slots.Count) {
                        continue;
                    }

                    slot = _slots[id++];
                    slot.Top.Set(yoffset, 0);
                    slot.Left.Set(xoffset, 0);
                    xoffset += slot.Width.Pixels + spacing;
                    Panel.Append(slot);
                }

                yoffset += slot.Height.Pixels + spacing;
                y -= slot.Height.Pixels + spacing;
            }
            
            this.Redrawing = false;
            //this.Append(Panel);
        }

        public void TurnLeft() {
            this.SetPage(Math.Max(0, lastpage - 1));
        }

        public void TurnRight() {
            this.SetPage(Math.Min(Slots.Count / ItemsPer, lastpage + 1));
        }

        private string _old_filter = "";
        public void SetPage(int page = 0) {
            this.Redraw(ItemsPer * page, _old_filter);
            lastpage = page;
        }

        public override void OnInitialize() {
            base.OnInitialize();
            //var _panel = new UIPanel();//Main.screen
            Panel = new DraggableUIPanel() {
                HAlign = 0.5f,
                VAlign = 0.5f,
            };

            SearchBar = new SearchBar() {
                HAlign = 0.5f,
            };

            SearchBar.OnChange += this.OnSort;

            this.Append(Panel);
        }

        public override void ExecuteRecursively(UIElementAction action) {
            if (!this.Redrawing) {
                base.ExecuteRecursively(action);
            }
        }

        public override void PostUpdateInput(GameTime gameTime) {
            if (!this.Redrawing) {
                base.PostUpdateInput(gameTime);
            }    
        }
    }

    // Draw Code Taken From https://github.com/JavidPack/CheatSheet/blob/1.4/Menus/Slot.cs and Terrara.UI.UIImageButton
    internal class CheatItemSlot: UIElement {

        public static CheatItemSlot Default { get; } = new(0);
        protected static Asset<Texture2D> Background;

        Item Item {
            get; init;
        }

        public CheatItemSlot(Asset<Texture2D> background, Item item) {
            this.Item = item;
            Background = background;
            this.Height.Set(Background.Height(), 0);
            this.Width.Set(Background.Width(), 0);
            Task.Run(() => { Main.instance.LoadItem(item.type); });
        }

        public CheatItemSlot(Item item) : this(TextureAssets.InventoryBack15, item) { }

        public CheatItemSlot(Asset<Texture2D> background, int type) : this(background, item: new(type)) { }

        public CheatItemSlot(int type) : this(item: new(type)) { }

        public string GetItemName() {
            return Item?.Name;
        }

        public override void Click(UIMouseEvent evt) {
            base.Click(evt);
            Click();
        }

        public void Click() {
            var shift = Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);
            var control = Main.keyState.IsKeyDown(Keys.LeftControl) || Main.keyState.IsKeyDown(Keys.RightControl);

            if (control || !Main.playerInventory) {
                Main.LocalPlayer.QuickSpawnClonedItemDirect(new EntitySource_Misc($"{TestMod.Instance.Name}: Cheat Menu"), Item, shift ? Item.stack : 1);
                return;
            }
            if (Main.mouseItem.type == ItemID.None) {
                Main.mouseItem = Item.Clone();
                Main.mouseItem.stack = shift ? Item.maxStack : 1;
            } else if (Main.mouseItem.type == Item.type) {
                //Main.mouseItem.netDefaults(Item.netID);
                Main.mouseItem.stack = shift ? Item.maxStack : Math.Min(Main.mouseItem.stack + 1, Item.maxStack);
                Main.mouseItem.Refresh();
            } else {
                Main.mouseItem = new Item(ItemID.None).Clone();
            }

            SoundEngine.PlaySound(SoundID.Coins);
        }

        //int baseDelay = 1;
        //float hasHeld = 0;
        //GameTime _lastGameTime = new GameTime();
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (IsMouseHovering && Main.mouseRight) {
                Click();
            }
            //float delay = baseDelay - hasHeld;

            //if (IsMouseHovering) {
            //    var d = _lastGameTime.TotalGameTime.Add(TimeSpan.FromSeconds(1)).TotalSeconds - gameTime.TotalGameTime.TotalSeconds;
            //    if (Main.mouseLeft && _lastGameTime.TotalGameTime.Add(TimeSpan.FromSeconds(1)).TotalSeconds - gameTime.TotalGameTime.TotalSeconds <= delay) {
            //        Click();
            //        hasHeld -= .2f;
            //        _lastGameTime = gameTime;
            //    } else if (!Main.mouseLeft) {
            //        hasHeld = 0;
            //    } else if (hasHeld > 0) {
            //        hasHeld -= .2f;
            //    } else {
            //        hasHeld = 1;
            //    }
            //}

        }

        int counter = 0;

        protected override void DrawSelf(SpriteBatch spriteBatch) {

            var dimentions = GetDimensions().Position();
            //Main.instance.LoadItem(Item.type);

            spriteBatch.Draw(Background.Value, dimentions, Color.White);

            var forground = TextureAssets.Item[Item.type];


            //var imgSize = forground.Frame(1, 1);
            //spriteBatch.Draw(forground.Value, new Vector2(dimentions.X + Background.Height() / 2 - imgSize.Height / 2, dimentions.Y + Background.Width() / 2 - imgSize.Width / 2), imgSize, Color.White);


            if (forground == null) {
                return;
            }

            Rectangle rectangle2;
            if (Main.itemAnimations[Item.type] != null) {
                rectangle2 = Main.itemAnimations[Item.type].GetFrame(forground.Value);
            } else {
                rectangle2 = forground.Value.Frame(1, 1, 0, 0);
            }
            float num = 1f;
            float Scale = 1f;
            float num2 = Background.Width() * Scale * 0.6f;
            if (rectangle2.Width > num2 || rectangle2.Height > num2) {
                if (rectangle2.Width > rectangle2.Height) {
                    num = num2 / rectangle2.Width;
                } else {
                    num = num2 / rectangle2.Height;
                }
            }
            Vector2 drawPosition = dimentions;
            drawPosition.X += Background.Width() * Scale / 2f - rectangle2.Width * num / 2f;
            drawPosition.Y += Background.Height() * Scale / 2f - rectangle2.Height * num / 2f;
            this.Item.GetColor(Color.White);
            spriteBatch.Draw(forground.Value, drawPosition, new Rectangle?(rectangle2), this.Item.GetAlpha(Color.White), 0f, Vector2.Zero, num, SpriteEffects.None, 0f);

            if (IsMouseHovering) {
                Main.hoverItemName = $"{Item.Name} (from {(Item.ModItem != null ? Item.ModItem.Mod.Name : "Terraria")})";
            }
        }

        public override void MouseOver(UIMouseEvent evt) {
            base.MouseOver(evt);
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public string GetItemModName() {
            return Item?.ModItem?.Name ?? "Terraria";
        }

    }
}
