using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace TestMod.UI.Common {
    public class BaseUIState: UIState {

        public virtual void PostUpdateInput(GameTime gameTime) {
            foreach (var _child in this.Children) {
                if (_child is BaseUIElement child && child != null) {
                    child.PostUpdateInput(gameTime);
                }

                var _child2 = _child.Children;
                for (var i = 0; i < _child2.Count(); i++) {
                    _child2.ToImmutableList()[i].ExecuteRecursively(e => {
                        if (e is BaseUIElement child && child != null) {
                            child.PostUpdateInput(gameTime);
                        }
                    });
                }
            }
        }


    }
}
