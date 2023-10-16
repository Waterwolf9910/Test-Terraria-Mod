using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;
using TestMod.UI.Common;
using TestMod.Utils;

namespace TestMod.UI.Elements {

    // Some Coding Taken from https://github.com/blushiemagic/MagicStorage/blob/1.4/UI/UISearchBar.cs
    public class SearchBar: BaseUIElement {
        public string Text {
            get;
            protected set;
        } = "";

        public event Action<string> OnChange;

        protected bool Focused = false;
        protected int CursorPos {
            get => _curpos;
            set => _curpos = Math.Max(Math.Min(value, Text.Length), 0);
        }
        protected static Asset<DynamicSpriteFont> TextFont = FontAssets.MouseText;
        protected Asset<Texture2D> Background = TextureAssets.TextBack;
        private int _curpos = 0;

        public SearchBar() {
            this.Height.Pixels = Background.Height() * .75f;
        }

        public SearchBar(Asset<Texture2D> background, float scale) {
            this.Height.Pixels = Background.Height() * scale;
        }

        public SearchBar(Asset<Texture2D> background): this(background, 1f) {
            
        }

        public void Clear() {
            this.CursorPos = 0;
            this.Text = "";
            this.Focused = true;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

        }

        public override void PostUpdateInput(GameTime gameTime) {
            base.PostUpdateInput(gameTime);
            if (Main.mouseLeft || Main.mouseRight) {
                this.Focused = this.IsMouseHovering;
            }

            if (Util.IsKeyDown(Keys.Tab) || Util.IsKeyDown(Keys.Enter)) {
                this.Focused = true;
            }

            if (this.Focused) {
                //this.HandleTextInput();
                this.HandleKeyInput();
            }
        }

        protected int _ctr = 15;
        protected int _ctrTime { get; } = 15;

        public void HandleKeyInput() {
            //var control = Main.keyState.IsKeyDown(Keys.LeftControl) || Main.keyState.IsKeyDown(Keys.RightControl);
            //var back = Main.keyState.IsKeyDown(Keys.Back);
            //var del = Main.keyState.IsKeyDown(Keys.Delete);

            //Main.clrInput();
            PlayerInput.WritingText = true;
            //Main.blockInput = true;
            Main.instance.HandleIME();

            var _oldText = Text;
            var oldText = _oldText;

            if (Text.Length > 0) {
                oldText = oldText.Remove(CursorPos);
            }

            var newText = Main.GetInputText(oldText).ToLower();
            var _curpos = newText.Length;
            Text = newText + Text[CursorPos..];
            CursorPos = _curpos;
            if ((Util.IsKeyDown(Keys.Delete) || (Main.keyState.IsKeyDown(Keys.Delete) && _ctr == 0)) && Text.Length > CursorPos) {

                if (Main.keyState.IsKeyDown(Keys.LeftControl)) {
                    Text = Text[..CursorPos] + string.Join(" ", Text[CursorPos..].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..]);
                } else {
                    Text = Text.Remove(CursorPos, 1);
                }
            }

            if (_oldText != Text) {
                this.OnChange?.Invoke(Text);
            }
            //if (_text.Length == Text.Length || _text.Length < 1) {
            //    return;
            //}

            //if (Text.Length > 0) {
            //    Text = Text.Remove(CursorPos, 1);
            //}

            if (Util.IsKeyDown(Keys.Left) || (Main.keyState.IsKeyDown(Keys.Left) && _ctr == 0)) {
                if (Main.keyState.IsKeyDown(Keys.LeftControl)) {
                    CursorPos -= Text[..CursorPos].Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1].Length + 1;
                } else {
                    CursorPos--;
                }
            }

            if (Util.IsKeyDown(Keys.Right) || (Main.keyState.IsKeyDown(Keys.Right) && _ctr == 0)) {
                if (Main.keyState.IsKeyDown(Keys.LeftControl)) {
                    CursorPos += Text[CursorPos..].Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Length + 1;
                } else {
                    CursorPos++;
                }
            }

            if (Util.IsKeyDown(Keys.Home)) {
                CursorPos = 0;
            }

            if (Util.IsKeyDown(Keys.End)) {
                CursorPos = Text.Length;
            }

            if (Main.keyState.IsKeyDown(Keys.Delete) || Main.keyState.IsKeyDown(Keys.Left) || Main.keyState.IsKeyDown(Keys.Right)) {
                --_ctr;
                _ctr %= _ctrTime;
            } else {
                _ctr = _ctrTime;
            }
            

            if (Main.keyState.IsKeyDown(Keys.Enter) || Main.keyState.IsKeyDown(Keys.Tab)) {
                this.Focused = false;
                Main.blockInput = false;
            }
        }

        private int blink_ctr = 30;
        private bool cursor = true;
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            var dimentions = GetDimensions();

            //var rect = Background.Value.Frame(1, 1, 0, 0);
            var x = this.Width.Pixels / this.Parent.Width.Pixels;
            var y = this.Height.Pixels / this.Background.Height();
            Vector2 v = new(x, y);
            Vector2 textpos = new(dimentions.Position().X * 1.015f, dimentions.Position().Y * 1.005f);
            //rect.Width = (int)dimentions.Width;
            //spriteBatch.Draw(Background.Value, dimentions.Position(), rect, Color.White);
            spriteBatch.Draw(Background.Value, dimentions.Position(), null, Color.White * (this.Focused ? 1f : 0.5f), 0f, Vector2.Zero, v, SpriteEffects.None, 0f);
            spriteBatch.DrawString(TextFont.Value, Text.Insert(this.CursorPos, this.Focused ? this.cursor ? "|" : " " : "|"), textpos, Color.White * (this.Focused ? 1f : 0.5f));

            blink_ctr--;
            blink_ctr %= 30;
            if (blink_ctr == 0) {
                cursor = !cursor;
            }
            if (this.IsMouseHovering || base.IsMouseHovering) {
                //Console.WriteLine("");
            }

        }

        public override void MouseOver(UIMouseEvent evt) {
            base.MouseOver(evt);
        }
    }
}
