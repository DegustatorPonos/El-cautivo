using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework.Content;

namespace El_Cautivo
{
    static class MainMenu
    {
        public static void INIT(Action exit)
        {
            Quit = exit;
        }

        static void InitButtons()
        {
            ExitButton = new Button(ExitPosition/ Game1.dScale, ExitButtonTexture, Quit, 4/ Game1.dScale);
        }
        static Action Quit;
        public static void LoadContent(ContentManager Content)
        {
            BG = Content.Load<Texture2D>("MenuBG");
            ExitButtonTexture = Content.Load<Texture2D>("Buttons/ExitButton");
            InitButtons();
        }
        
        static Texture2D BG, BeginButtontexture, ExitButtonTexture, SettingsButtonTexture;
        public static SpriteFont Font { get; set; }

        #region GUI elements
        static Vector2 ExitPosition = new Vector2(50, 780);

        static Button ExitButton;
        #endregion

        public static void Draw(SpriteBatch batch)
        {
            //batch.Draw(BG, Vector2.Zero, new Rectangle(0,0,192,108), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, BG.Width, BG.Height), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
            //batch.DrawString(Font, Mouse.GetState().Position.ToString(), Vector2.Zero, Color.White); //Title
            ExitButton.Draw(batch);
            
        }

        public static void Update()
        {
            ExitButton.Update();
        }
    }
}
