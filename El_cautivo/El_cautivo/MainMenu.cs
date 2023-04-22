using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace El_Cautivo
{
    static class MainMenu
    {
        public static Texture2D BG { get; set; }
        public static SpriteFont Font { get; set; }
        static Vector2 TitlePosition = new Vector2(64, 64);
        public static void Draw(SpriteBatch batch)
        {
            //batch.Draw(BG, Vector2.Zero, new Rectangle(0,0,192,108), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, 192, 168), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
            //batch.DrawString(Font, "El Cautivo", TitlePosition, Color.White); //Title
        }
    }
}
