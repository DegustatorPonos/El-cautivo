using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using El_Cautivo.Menus;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo
{
    static class Lab
    {
        public static Texture2D Surroundings;

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(Surroundings, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
        }
        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game1.state = Game1.GameState.ExitMenu;
        }
    }
}
