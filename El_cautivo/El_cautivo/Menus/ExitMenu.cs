using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace El_Cautivo.Menus
{
    public static class ExitMenu
    {
        #region GUI elements
        static Texture2D BG, Confirm, Decline, Text;
        static Button DeclineButton, ConfirmButton;
        static Vector2
            DeclinePos = new Vector2(1636, 824),
            ConfirmPos = new Vector2(0, 824);
        public static void LoadContent(ContentManager Content)
        {
            BG = Content.Load<Texture2D>("Lab");
            Text = Content.Load<Texture2D>("Buttons/ExitMenu/Text");
            Decline = Content.Load<Texture2D>("Buttons/ExitMenu/Decline");
            Confirm = Content.Load<Texture2D>("Buttons/ExitMenu/Confirm");
            InitButtons();
        }

        static void InitButtons()
        {
            DeclineButton = new Button(DeclinePos / Game1.dScale, Decline, new Action(() => Game1.state = Game1.GameState.Game), 4/Game1.dScale, Button.ButtonType.OnUp);
            ConfirmButton = new Button(ConfirmPos / Game1.dScale, Confirm, new Action(() => Game1.state = Game1.GameState.MainMenu), 4 / Game1.dScale, Button.ButtonType.OnUp);
        }

        #endregion

        public static void Update()
        {
            ConfirmButton.Update();
            DeclineButton.Update();
        }

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, BG.Width, BG.Height), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            batch.Draw(Text, Vector2.Zero, new Rectangle(0, 0, Text.Width, Text.Height), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            ConfirmButton.Draw(batch);
            DeclineButton.Draw(batch);
        }
    }
}
