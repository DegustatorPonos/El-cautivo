using System;
using El_Cautivo.EngineExtentions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace El_Cautivo.Menus
{
    public static class SettingsMenu
    {
        #region GUI elements
        static Button BackButton, FSButton;
        static Texture2D BackTexture, SliderTexture, SliderBasisTexture, FullScreenTexture;
        static Slider VolumeSlider;
        static Texture2D BG;
        public static void LoadContent(ContentManager Content)
        {
            BG = Content.Load<Texture2D>("BG_clean");
            BackTexture = Content.Load<Texture2D>("Buttons/BackButton");
            FullScreenTexture = Content.Load<Texture2D>("Buttons/FullScreenButton");
            SliderTexture = Content.Load<Texture2D>("SliderPointer");
            SliderBasisTexture = Content.Load<Texture2D>("SliderBasis");
            InitButtons();
        }
        static Vector2 BackPos = new Vector2(10, 814);
        static Vector2 FSPos = new Vector2(1654, 412);
        static Vector2 SliderPos = new Vector2(10, 540);
        public static Action FullScreenManager;
        static void InitButtons()
        {
            BackButton = new Button(BackPos/Game1.dScale, BackTexture, new Action(() => Game1.state = Game1.GameState.MainMenu),
                4/Game1.dScale, Button.ButtonType.OnUp);
            FSButton = new Button(FSPos/Game1.dScale, FullScreenTexture, FullScreenManager,
                4/Game1.dScale, Button.ButtonType.OnUp);
            VolumeSlider = new Slider(SliderBasisTexture, SliderTexture, SliderPos/Game1.dScale);
        }
        #endregion
        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(BG, Vector2.Zero, new Rectangle(0, 0, BG.Width, BG.Height), Color.White, 0, Vector2.Zero, new Vector2(10, 10) / Game1.dScale, SpriteEffects.None, 0);
            if (Game1.IsFSAvaliable) FSButton.Draw(batch);
            VolumeSlider.Draw(batch);
            BackButton.Draw(batch);
        }

        public static void Update()
        {
            VolumeSlider.Update();
            Game1.MusicVolume = VolumeSlider.Value;
            if (Game1.IsFSAvaliable) FSButton.Update();
            BackButton.Update();
        }
    }
}
