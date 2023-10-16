using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace TestMod.UI.Common {
    public class BaseUIElement: UIElement {

        public virtual void PostUpdateInput(GameTime gameTime) {
            //foreach (var _child in this.Children) {
            //    if (_child is BaseUIElement child && child != null) {
            //        child.PostUpdateInput(gameTime);
            //    }
            //}
        }
    }
}
