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
using El_Cautivo.GameObjects;
using Microsoft.Xna.Framework.Audio;

namespace El_Cautivo
{
    static class MainMenu
    {
        #region GUI elements
        public static void Init(Action exit)
        {
            Quit = exit;
        }
        static void InitButtons()
        {
            ExitButton = new Button(ExitPosition/ Game1.dScale, ExitButtonTexture, Quit, 4/ Game1.dScale);
            BeginButton = new Button(BeginningPosition / Game1.dScale, BeginButtontexture, BeginGame, 4 / Game1.dScale, Button.ButtonType.OnUp);
            SettingsButton = new Button(SettingsPosition / Game1.dScale, SettingsButtonTexture, new Action(() => Game1.state = Game1.GameState.SettingsMenu), 4 / Game1.dScale);
        }
        public static Action Quit;
        static ContentManager content;
        static SoundEffect radio;
        public static void LoadContent(ContentManager Content)
        {
            radio = Content.Load<SoundEffect>("Audio/Radio");
            content = Content;
            BG = Content.Load<Texture2D>("MenuBG");
            ExitButtonTexture = Content.Load<Texture2D>("Buttons/ExitButton");
            BeginButtontexture = Content.Load<Texture2D>("Buttons/BeginButton");
            SettingsButtonTexture = Content.Load<Texture2D>("Buttons/SettingsButton");
            InitButtons();
        }

        static Texture2D BG, BeginButtontexture, ExitButtonTexture, SettingsButtonTexture;
        public static SpriteFont Font { get; set; }

        
        static Vector2 ExitPosition = new Vector2(50, 780);
        static Vector2 SettingsPosition = new Vector2(832, 780);
        static Vector2 BeginningPosition = new Vector2(1614, 780);

        static Button ExitButton, BeginButton, SettingsButton;
        #endregion

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, BG.Width, BG.Height), Color.White, 0, Vector2.Zero, new Vector2(10, 10)/Game1.dScale, SpriteEffects.None, 0);
            
            //batch.DrawString(Font, Mouse.GetState().Position.ToString(), Vector2.Zero, Color.White); //Mouse coorinates
            ExitButton.Draw(batch);
            BeginButton.Draw(batch);
            SettingsButton.Draw(batch);
        }

        public static void Update()
        {
            ExitButton.Update();
            BeginButton.Update();
            SettingsButton.Update();
        }

        static void BeginGame()
        {
            Game1.InitBGM(radio);
            Game1.CurrencCS = new CutScene(content, Game1.ChatPairs, Cutscenes.Beginning, EndBeginningCS);
            Game1.state = Game1.GameState.CutScene;
            
        }

        static void EndBeginningCS()
        {
            Lab.BeginDay();
            Game1.state = Game1.GameState.Game;
        }
    }
}
